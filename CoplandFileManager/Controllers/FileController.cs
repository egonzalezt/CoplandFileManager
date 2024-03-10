namespace CoplandFileManager.Controllers;

using Application.Interfaces;
using CoplandFileManager.Responses;
using Domain.File.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class FileController(ICreateFileUseCase createFileUseCase, IGetSignedUrlUseCase getSignedUrlUseCase) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<ActionResult<BaseResponse<FileCreatedResponse>>> UploadFileAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] Guid userId, IFormFile file)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("User Id not found");
        }
        if (file is null || file.Length == 0)
        {
            return BadRequest("File not found");
        }

        var fileDto = new FileDto
        {
            NameWithExtension = file.FileName,
            MimeType = file.ContentType.ToLower(),
        };
        await createFileUseCase.TryCreateAsync(file.OpenReadStream(), fileDto, userId);

        var fileCreatedResponse = new FileCreatedResponse
        {
            FileName = file.FileName
        };

        var response = new BaseResponse<FileCreatedResponse>
        {
            Content = fileCreatedResponse,
            Message = "Lain uploaded the file on the system"
        };

        return Ok(response);
    }

    [HttpGet("get-file-url")]
    public async Task<ActionResult<BaseResponse<SignedUrlResponse>>> GetPreSignedUrlAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] Guid userId, [FromQuery] Guid fileId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("User Id not found");
        }
        (var url, var timeLimit) = await getSignedUrlUseCase.GetSignedUrlUseCaseAsync(userId, fileId);
        var signedResponse = new SignedUrlResponse
        {
            TimeLimit = timeLimit,
            Url = url
        };
        var baseResponse = new BaseResponse<SignedUrlResponse> { Content = signedResponse, Message = "Signed Url generated to get the file" };
        return Ok(baseResponse);
    }

    [HttpPost("upload-file-signed-url")]
    public async Task<ActionResult<BaseResponse<SignedUrlResponse>>> GeneratePreSignedUrlForUploadUrlAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] Guid userId, [FromBody] FileDto fileDto)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("User Id not found");
        }
        (var url, var timeLimit) = await createFileUseCase.TryCreateAsync(fileDto, userId);
        var signedResponse = new SignedUrlResponse
        {
            TimeLimit = timeLimit,
            Url = url
        };
        var baseResponse = new BaseResponse<SignedUrlResponse> { Content = signedResponse, Message = "Signed Url generated to upload the file" };
        return Ok(baseResponse);
    }
}
