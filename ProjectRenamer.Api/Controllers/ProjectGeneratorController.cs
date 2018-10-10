using System;
using System.Net;
using Alternatives;
using Alternatives.CustomExceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProjectRenamer.Api.Helper;
using ProjectRenamer.Api.Requests;
using ProjectRenamer.Api.Responses;

namespace ProjectRenamer.Api.Controllers
{
    public class ProjectGeneratorController : Controller
    {
        private readonly ValidatorResolver _validatorResolver;
        private const string CONTENT_TYPE = "application/zip";

        public ProjectGeneratorController(ValidatorResolver validatorResolver)
        {
            _validatorResolver = validatorResolver;
        }

        [HttpPost, Route("download")]
        public FileContentResult Download([FromBody] DownloadProjectRequest request)
        {
            var downloadProjectRequestValidator = _validatorResolver.Resolve<DownloadProjectRequestValidator>();
            ValidationResult validationResult = downloadProjectRequestValidator.Validate(request);
            Guard.IsFalse(validationResult.IsValid, new CustomApiException(validationResult.ToString(), HttpStatusCode.BadRequest));

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
            var generateProjectOverGitRequestValidator = _validatorResolver.Resolve<GenerateProjectOverGitRequestValidator>();
            ValidationResult validationResult = generateProjectOverGitRequestValidator.Validate(generateProjectOverGitRequest);
            Guard.IsFalse(validationResult.IsValid, new CustomApiException(validationResult.ToString(), HttpStatusCode.BadRequest));

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
            var generateProjectOverZipRequestValidator = _validatorResolver.Resolve<GenerateProjectOverZipRequestValidator>();
            ValidationResult validationResult = generateProjectOverZipRequestValidator.Validate(generateProjectWithGivenZipRequest);
            Guard.IsFalse(validationResult.IsValid, new CustomApiException(validationResult.ToString(), HttpStatusCode.BadRequest));

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