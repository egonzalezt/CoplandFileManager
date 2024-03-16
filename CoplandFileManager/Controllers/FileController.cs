namespace CoplandFileManager.Controllers;

using Application.Interfaces;
using CoplandFileManager.Domain.File;
using CoplandFileManager.Domain.File.Dtos;
using CoplandFileManager.Domain.User.Dtos;
using CoplandFileManager.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class FileController(ICreateFileUseCase createFileUseCase, IGetSignedUrlUseCase getSignedUrlUseCase, IGetFilesUseCase getFilesUseCase) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<ActionResult<BaseResponse<FileCreatedResponse>>> UploadFileAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] string userInfoHeader, IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("File not found");
        }
        if (string.IsNullOrEmpty(userInfoHeader))
        {
            return BadRequest("User Id not found");
        }

        var userInfo = GetUserInfoAuthFromHeader(userInfoHeader);
        if (userInfo == null || userInfo.UserId == Guid.Empty)
        {
            return BadRequest("Invalid user info");
        }

        var fileDto = new FileUploadDto
        {
            NameWithExtension = file.FileName,
            MimeType = file.ContentType.ToLower(),
        };
        await createFileUseCase.TryCreateAsync(file.OpenReadStream(), fileDto, userInfo.UserId);

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
    public async Task<ActionResult<BaseResponse<SignedUrlResponse>>> GetPreSignedUrlAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] string userInfoHeader, [FromQuery] Guid fileId)
    {
        if (string.IsNullOrEmpty(userInfoHeader))
        {
            return BadRequest("User Id not found");
        }

        var userInfo = GetUserInfoAuthFromHeader(userInfoHeader);
        if (userInfo == null || userInfo.UserId == Guid.Empty)
        {
            return BadRequest("Invalid user info");
        }


        (var url, var timeLimit) = await getSignedUrlUseCase.GetSignedUrlUseCaseAsync(userInfo.UserId, fileId);
        var signedResponse = new SignedUrlResponse
        {
            TimeLimit = timeLimit,
            Url = url
        };
        var baseResponse = new BaseResponse<SignedUrlResponse> { Content = signedResponse, Message = "Signed Url generated to get the file" };
        return Ok(baseResponse);
    }

    [HttpPost("upload-file-signed-url")]
    public async Task<ActionResult<BaseResponse<SignedUrlResponse>>> GeneratePreSignedUrlForUploadUrlAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] string userInfoHeader, [FromBody] FileUploadDto fileDto)
    {
        if (string.IsNullOrEmpty(userInfoHeader))
        {
            return BadRequest("User Id not found");
        }

        var userInfo = GetUserInfoAuthFromHeader(userInfoHeader);
        if (userInfo == null || userInfo.UserId == Guid.Empty)
        {
            return BadRequest("Invalid user info");
        }

        (var url, var timeLimit) = await createFileUseCase.TryCreateAsync(fileDto, userInfo.UserId);
        var signedResponse = new SignedUrlResponse
        {
            TimeLimit = timeLimit,
            Url = url
        };
        var baseResponse = new BaseResponse<SignedUrlResponse> { Content = signedResponse, Message = "Signed Url generated to upload the file" };
        return Ok(baseResponse);
    }

    [HttpGet("get-files")]
    public async Task<ActionResult<PaginationResult<FileDto>>> GetFilesAsync([FromHeader(Name = "X-Apigateway-Api-Userinfo")] string userInfoHeader, [FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        if (string.IsNullOrEmpty(userInfoHeader))
        {
            return BadRequest("User Id not found");
        }

        var userInfo = GetUserInfoAuthFromHeader(userInfoHeader);
        if (userInfo == null || userInfo.UserId == Guid.Empty)
        {
            return BadRequest("Invalid user info");
        }

        var result = await getFilesUseCase.GetAsync(userInfo.UserId, pageIndex, pageSize);
        Response.Headers.Add("X-Pagination-Current-Page", result.CurrentPage.ToString());
        Response.Headers.Add("X-Pagination-Next-Page", result.NextPage.ToString());
        Response.Headers.Add("X-Pagination-Has-Next-Page", result.HasNextPage.ToString());
        Response.Headers.Add("X-Pagination-Total-Pages", result.TotalPages.ToString());
        return Ok(result.Data);
    }

    private UserInfoAuthDto? GetUserInfoAuthFromHeader(string header)
    {
        var decodedBytes = Convert.FromBase64String(header);
        var decodedString = Encoding.UTF8.GetString(decodedBytes);
        var userInfo = JsonSerializer.Deserialize<UserInfoAuthDto>(decodedString);
        return userInfo;
    }
}
