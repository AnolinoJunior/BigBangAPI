using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BigBangAPI.DTO;
using BigBangAPI.Models;
using BigBangAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        #region Roles

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response
                        {
                            Status = "Success",
                            Message = $"A função '{roleName}' foi criada com sucesso."
                        });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response
                        {
                            Status = "Error",
                            Message = $"Não foi possível criar a função '{roleName}'."
                        });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                new Response
                {
                    Status = "Error",
                    Message = "Essa função já existe."
                });
        }
            [Authorize(Policy = "AdminOnly")]
            [HttpPost]
            [Route("AddUserToRole")]
            public async Task<IActionResult> AddUserToRole(string email, string roleName)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);

                    if (result.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status200OK,
                            new Response
                            {
                                Status = "Success",
                                Message = $"O usuário {user.Email} foi adicionado à função {roleName}."
                            });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response
                            {
                                Status = "Error",
                                Message = $"Não foi possível adicionar o usuário {user.Email} à função {roleName}."
                            });
                    }
                }

                return BadRequest(new
                {
                    Error = "Não foi possível localizar o usuário."
                });
            }
        
        #endregion
    
         #region Login

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByNameAsync(model.UserName!);

                if (user == null ||
                    !await _userManager.CheckPasswordAsync(user, model.Password!))
                {
                    return Unauthorized(new Response
                    {
                        Status = "Error",
                        Message = "Nome de usuário ou senha inválidos."
                    });
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!)
                };

                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
                }

                authClaims.Add(
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var accessToken = _tokenService.GenerateAccessToken(
                    authClaims,
                    _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(
                    _configuration["JWT:RefreshTokenValidityInMinutes"],
                    out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime =
                    DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    return BadRequest(new
                    {
                        Status = "Error",
                        Message = "Não foi possível atualizar o usuário.",
                        Errors = updateResult.Errors.Select(e => e.Description)
                    });
                }

                return Ok(new TokenModel
                {
                    AccessToken = new JwtSecurityTokenHandler()
                        .WriteToken(accessToken),

                    RefreshToken = refreshToken,

                    Expiration = accessToken.ValidTo
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    Message = "Ocorreu um erro inesperado ao processar a solicitação de entrada."
                });
            }
        }

        #endregion

        #region Register

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists =
                await _userManager.FindByNameAsync(model.UserName!);

            if (userExists != null)
            {
                return Conflict(new Response
                {
                    Status = "Error",
                    Message = "Esse usuário já existe."
                });
            }

            var emailExists =
                await _userManager.FindByEmailAsync(model.Email!);

            if (emailExists != null)
            {
                return Conflict(new Response
                {
                    Status = "Error",
                    Message = "Esse e-mail já está cadastrado."
                });
            }

            ApplicationUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result =
                await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = "Não foi possível criar o usuário.",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new Response
            {
                Status = "Success",
                Message = "Usuário criado com sucesso."
            });
        }

        #endregion

        #region Refresh Token

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string accessToken = tokenModel.AccessToken
                ?? throw new ArgumentNullException(nameof(tokenModel.AccessToken));

            string refreshToken = tokenModel.RefreshToken
                ?? throw new ArgumentNullException(nameof(tokenModel.RefreshToken));

            var principal = _tokenService.GetPrincipalFromExpiredToken(
                accessToken,
                _configuration);

            string username = principal.Identity!.Name!;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = "Token de acesso ou token de atualização inválido."
                });
            }

            var newAccessToken = _tokenService.GenerateAccessToken(
                principal.Claims.ToList(),
                _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(
                _configuration["JWT:RefreshTokenValidityInMinutes"],
                out int refreshTokenValidityInMinutes);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok(new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler()
                    .WriteToken(newAccessToken),

                RefreshToken = newRefreshToken,

                Expiration = newAccessToken.ValidTo
            });
        }

        #endregion

        #region Revoke

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = "Nome de usuário inválido."
                });
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        #endregion
    }
    
}
