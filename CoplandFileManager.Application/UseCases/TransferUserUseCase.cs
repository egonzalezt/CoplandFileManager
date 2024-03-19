namespace CoplandFileManager.Application.UseCases;

using Domain.File.Dtos;
using Domain.File.Repositories;
using Domain.User.Repository;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class TransferUserUseCase(IFileCommandRepository fileCommandRepository, IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository, IGetSignedUrlUseCase getSignedUrlUseCase) : ITransferUserUseCase
{
    public async Task<(IEnumerable<UserFileTransferDto> files, bool userDeactivated)> ExecuteAsync(Guid userId)
    {
        var user = await userCommandRepository.GetByIdAsync(userId);
        if(user is null)
        {
            return ([], false);
        }
        user.UnActiveUser();
        userQueryRepository.Update(user);

        var files = new List<FileDto>();
        var transferFiles = new List<UserFileTransferDto>();
        var pageIndex = 0;
        var pageSize = 100;
        var timeLimit = TimeSpan.FromHours(48);
        while (true)
        {
            var pagedFiles = await fileCommandRepository.GetTransferFilesByUserIdAsync(userId, pageIndex, pageSize);
            files.AddRange(pagedFiles.Data);
            if (!pagedFiles.HasNextPage)
            {
                break;
            }
            pageIndex++;
        }

        foreach (var file in files)
        {
            (var url, _) = await getSignedUrlUseCase.GetSignedUrlForTransferAsync(userId, file.ObjectRoute, timeLimit);
            transferFiles.Add(new UserFileTransferDto { Id = file.Id, DocumentTitle = file.Name, UrlDocument = url });
        }

        return (transferFiles, true);
    }
}
