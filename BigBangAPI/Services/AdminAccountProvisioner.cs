using BigBangAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BigBangAPI.Services;

public sealed record AdminProvisioningResult(
    bool RoleCreated,
    bool UserCreated,
    bool RoleAssignmentCreated);

public static class AdminAccountProvisioner
{
    private const string AdminRoleName = "Admin";

    public static async Task<AdminProvisioningResult> ProvisionAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IPasswordHasher<ApplicationUser> passwordHasher,
        string userName,
        string email,
        string password)
    {
        userName = userName.Trim();
        email = email.Trim();

        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("O nome de usuário é obrigatório.", nameof(userName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório.", nameof(email));

        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("A senha é obrigatória.", nameof(password));

        var userByName = await userManager.FindByNameAsync(userName);
        var userByEmail = await userManager.FindByEmailAsync(email);

        if (userByName is not null &&
            userByEmail is not null &&
            userByName.Id != userByEmail.Id)
        {
            throw new InvalidOperationException(
                "O nome de usuário e o e-mail já pertencem a contas diferentes. Nenhuma alteração foi feita.");
        }

        var user = userByName ?? userByEmail;

        if (user is not null &&
            (!string.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase) ||
             !string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "Já existe uma conta com o nome de usuário ou e-mail informado, mas os dados não coincidem. Nenhuma alteração foi feita.");
        }

        if (user is not null)
        {
            var passwordHash = user.PasswordHash;
            var passwordVerification = string.IsNullOrEmpty(passwordHash)
                ? PasswordVerificationResult.Failed
                : passwordHasher.VerifyHashedPassword(user, passwordHash, password);

            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException(
                    "A conta já existe, mas a senha informada não corresponde. A senha existente não foi alterada.");
            }

            if (passwordVerification is not PasswordVerificationResult.Success and
                not PasswordVerificationResult.SuccessRehashNeeded)
            {
                throw new InvalidOperationException(
                    "Não foi possível validar a senha da conta existente.");
            }
        }

        var adminRole = await roleManager.FindByNameAsync(AdminRoleName);
        var roleCreated = false;

        if (adminRole is null)
        {
            var createRoleResult = await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
            adminRole = await roleManager.FindByNameAsync(AdminRoleName);

            if (adminRole is null)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar a role Admin: {FormatErrors(createRoleResult)}");
            }

            roleCreated = createRoleResult.Succeeded;
        }

        var userCreated = false;
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };

            var createUserResult = await userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar o usuário: {FormatErrors(createUserResult)}");
            }

            userCreated = true;
        }

        var roleAssignmentCreated = false;
        if (!await userManager.IsInRoleAsync(user, AdminRoleName))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(user, AdminRoleName);

            if (!addToRoleResult.Succeeded &&
                !await userManager.IsInRoleAsync(user, AdminRoleName))
            {
                throw new InvalidOperationException(
                    $"Não foi possível associar o usuário à role Admin: {FormatErrors(addToRoleResult)}");
            }

            roleAssignmentCreated = addToRoleResult.Succeeded;
        }

        return new AdminProvisioningResult(roleCreated, userCreated, roleAssignmentCreated);
    }

    private static string FormatErrors(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(error => error.Description));
}
