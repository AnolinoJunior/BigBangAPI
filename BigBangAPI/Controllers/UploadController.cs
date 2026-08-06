using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Verifica se um arquivo foi enviado
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    Message = "Nenhum arquivo foi enviado."
                });
            }

            // Permite apenas imagens
            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    Message = "Formato de imagem inválido."
                });
            }

            // Gera um nome único para o arquivo
            var fileName = $"{Guid.NewGuid()}{extension}";

            // Caminho físico onde será salvo
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images",
                "characters");

            // Cria a pasta caso ela não exista
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            // Salva o arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Caminho que será salvo no banco
            var imageUrl = $"/Images/characters/{fileName}";

            return Ok(new
            {
                ImageUrl = imageUrl
            });
        }
    }
}