using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alternatives.CustomExceptions;
using Alternatives.Extensions;
using LibGit2Sharp;
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
        public FileContentResult Download([FromBody]DownloadProjectRequest request)
        {
            SolutionGenerator solutionGenerater = new SolutionGenerator();
            byte[] zipBytes = solutionGenerater.Download(request.Token);

            DirectoryInfo directory = Directory.GetParent(Directory.GetCurrentDirectory());
            string zipPath = Path.Combine(directory.FullName, $"{request.Token}.zip");

            return new FileContentResult(zipBytes, CONTENT_TYPE)
            {
                FileDownloadName = zipPath
            };
        }

        [HttpPost, Route("generator")]
        public GenerateProjectResponse Generate([FromBody] GenerateProjectRequest generateProjectRequest)
        {
            if (!generateProjectRequest.IsValid(out string validationMessage))
            {
                throw new CustomApiException(validationMessage, HttpStatusCode.BadRequest);
            }

            CloneOptions cloneOptions = new CloneOptions
            {
                BranchName = generateProjectRequest.BranchName,
                CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = generateProjectRequest.UserName,
                    Password = generateProjectRequest.Password
                }
            };

            SolutionGenerator solutionGenerater = new SolutionGenerator();
            string token = solutionGenerater.Generate(generateProjectRequest.RepositoryLink, generateProjectRequest.ProjectName, generateProjectRequest.RenamePairs, cloneOptions);

            return new GenerateProjectResponse()
            {
                Token = token
            };
        }
    }
}