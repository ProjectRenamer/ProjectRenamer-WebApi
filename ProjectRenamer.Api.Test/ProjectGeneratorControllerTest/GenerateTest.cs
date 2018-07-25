using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Alternatives.CustomExceptions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProjectRenamer.Api.Controllers;
using ProjectRenamer.Api.Requests;
using ProjectRenamer.Api.Responses;
using Xunit;

namespace ProjectRenamer.Api.Test
{
    public class GenerateTest
    {
        const string validGitRepoAddress = "https://github.com/AdemCatamak/DotNet.Template.With.Angular.git";
        private ProjectGeneratorController _sut;
        public GenerateTest()
        {
            _sut = new ProjectGeneratorController();
        }

        [Theory]
        [MemberData(nameof(InvalidRequests))]
        public void WhenRequestIsInvalid_CustomApiExceptionOccurs(GenerateProjectRequest generateProjectRequest)
        {
            var customApiException = Assert.Throws<CustomApiException>(() => _sut.Generate(generateProjectRequest));
            Assert.Equal(HttpStatusCode.BadRequest, customApiException.ReturnHttpStatusCode);
        }


        [Fact]
        public void WhenLinkIsInvalid_CustomApiExceptionOccurs()
        {
            GenerateProjectRequest generateProjectRequest = new GenerateProjectRequest()
            {
                RepositoryLink = "invalid",
                RenamePairs = new List<KeyValuePair<string, string>>(),
                BranchName = "master"
            };
            var customApiException = Assert.Throws<CustomApiException>(() => _sut.Generate(generateProjectRequest));
            Assert.Equal(HttpStatusCode.BadRequest, customApiException.ReturnHttpStatusCode);
        }

        [Fact(Skip = "This function used during download test")]
        public string WhenOperationCompleted_FileShoulBeExistWithNameofToken()
        {
            GenerateProjectRequest generateProjectRequest = new GenerateProjectRequest()
            {
                RepositoryLink = validGitRepoAddress,
                RenamePairs = new List<KeyValuePair<string, string>>(),
                BranchName = "master"
            };
            GenerateProjectResponse response = _sut.Generate(generateProjectRequest);

            Assert.NotNull(response);
            Assert.NotEmpty(response.Token);

            string currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var directories = Directory.GetDirectories(currentDirectory, response.Token, SearchOption.AllDirectories);
            Assert.True(directories.Any());

            return response.Token;
        }

        public static List<object[]> InvalidRequests()
        {
            var result = new List<object[]>
            {
                new object[]{ null},
                new object[]{ new GenerateProjectRequest()},
                new object[]{ new GenerateProjectRequest{BranchName = "master", RenamePairs = new List<KeyValuePair<string, string>>()} },
                new object[]{ new GenerateProjectRequest{BranchName = "master", RepositoryLink = validGitRepoAddress } },
                new object[]{ new GenerateProjectRequest{ RenamePairs = new List<KeyValuePair<string, string>>() , RepositoryLink = validGitRepoAddress} },
            };

            return result;
        }
    }
}
