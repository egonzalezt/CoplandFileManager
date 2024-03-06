namespace CoplandFileManager.Controllers;

using CoplandFileManager.Application.Interfaces;
using Domain.File.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class FileController(ICreateFileUseCase createFileUseCase, IGetSignedUrlUseCase getSignedUrlUseCase) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] string userId, IFormFile file)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User Id not found");
        }
        if (file is null || file.Length == 0)
        {
            return BadRequest("File not found");
        }

        var fileDto = new FileDto
        {
            Format = Path.GetExtension(file.FileName),
            IdentityProviderUserId = userId,
            Name = file.FileName,
            MimeType = file.ContentType.ToLower(),
        };

        await createFileUseCase.TryCreateAsync(file.OpenReadStream(), fileDto);

        return Ok("File Uploaded");
    }

    [HttpGet("get-file-url")]
    public async Task<IActionResult> GetPreSignedUrlAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] string userId, [FromQuery] string objectId)
    {
        var url = await getSignedUrlUseCase.GetSignedUrlUseCaseAsync(userId, objectId); 
        return Ok(url);
    }
}
