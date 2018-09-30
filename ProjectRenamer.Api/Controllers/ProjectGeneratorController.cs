using System;
using System.Net;
using Alternatives.CustomExceptions;
using Alternatives.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectRenamer.Api.Helper;
using ProjectRenamer.Api.Requests;
using ProjectRenamer.Api.Responses;

namespace ProjectRenamer.Api.Controllers
{
    public class ProjectGeneratorController : Controller
    {
        private const string CONTENT_TYPE = "application/zip";


        [HttpPost, Route("download")]
        public FileContentResult Download([FromBody] DownloadProjectRequest request)
        {
            if (!request.IsValid(out string validationMessage))
            {
                throw new CustomApiException(validationMessage, HttpStatusCode.BadRequest);
            }

            var solutionGenerater = new SolutionGenerator();
            byte[] zipBytes = solutionGenerater.Download(request.Token);

            return new FileContentResult(zipBytes, CONTENT_TYPE)
                   {
                       FileDownloadName = request.Token
                   };
        }

        [HttpPost, Route("generator")]
        [Obsolete("generate-over-git endpoint should be used")]
        public GenerateProjectResponse Generate([FromBody] GenerateProjectOverGitRequest generateProjectOverGitRequest)
        {
            return GenerateOverGit(generateProjectOverGitRequest);
        }

        [HttpPost, Route("generate-over-git")]
        public GenerateProjectResponse GenerateOverGit([FromBody] GenerateProjectOverGitRequest generateProjectOverGitRequest)
        {
            if (!generateProjectOverGitRequest.IsValid(out string validationMessage))
            {
                throw new CustomApiException(validationMessage, HttpStatusCode.BadRequest);
            }


            var solutionGenerater = new SolutionGenerator();
            string fileName = solutionGenerater.DownloadRepoFromGit(generateProjectOverGitRequest.RepositoryLink, generateProjectOverGitRequest.BranchName, generateProjectOverGitRequest.UserName, generateProjectOverGitRequest.Password);
            string token = solutionGenerater.Generate(fileName, generateProjectOverGitRequest.RenamePairs);

            return new GenerateProjectResponse
                   {
                       Token = token
                   };
        }

        [HttpPost, Route("generate-over-zip")]
        public GenerateProjectResponse GenerateOverZip([FromForm] GenerateProjectOverZipRequest generateProjectWithGivenZipRequest)
        {
            if (!generateProjectWithGivenZipRequest.IsValid(out string validationMessage))
            {
                throw new CustomApiException(validationMessage, HttpStatusCode.BadRequest);
            }

            var solutionGenerater = new SolutionGenerator();
            string fileName = solutionGenerater.Upload(generateProjectWithGivenZipRequest.ZipFile);
            string token = solutionGenerater.Generate(fileName, generateProjectWithGivenZipRequest.RenamePairs);

            return new GenerateProjectResponse
                   {
                       Token = token
                   };
        }
    }
}