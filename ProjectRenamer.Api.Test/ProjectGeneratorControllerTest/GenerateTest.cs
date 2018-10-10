using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Alternatives.CustomExceptions;
using FluentValidation.Results;
using ProjectRenamer.Api.Controllers;
using ProjectRenamer.Api.Requests;
using ProjectRenamer.Api.Responses;
using ProjectRenamer.Api.Test.Fixture;
using Xunit;

namespace ProjectRenamer.Api.Test.ProjectGeneratorControllerTest
{
    public class GenerateTest : IClassFixture<ValidationFailure>
    {
        const string validGitRepoAddress = "https://github.com/AdemCatamak/DotNet.Template.With.Angular.git";
        private ProjectGeneratorController _sut;

        public GenerateTest(ValidationFixture validationFixture)
        {
            _sut = new ProjectGeneratorController(validationFixture.ValidatorResolver);
        }

        [Theory]
        [MemberData(nameof(InvalidRequests))]
        public void WhenRequestIsInvalid_CustomApiExceptionOccurs(GenerateProjectOverGitRequest generateProjectOverGitRequest)
        {
            var customApiException = Assert.Throws<CustomApiException>(() => _sut.GenerateOverGit(generateProjectOverGitRequest));
            Assert.Equal(HttpStatusCode.BadRequest, customApiException.ReturnHttpStatusCode);
        }


        [Fact]
        public void WhenLinkIsInvalid_CustomApiExceptionOccurs()
        {
            var generateProjectOverGitRequest = new GenerateProjectOverGitRequest()
                                                {
                                                    RepositoryLink = "invalid",
                                                    RenamePairs = new List<KeyValuePair<string, string>>(),
                                                    BranchName = "master"
                                                };
            var customApiException = Assert.Throws<CustomApiException>(() => _sut.GenerateOverGit(generateProjectOverGitRequest));
            Assert.Equal(HttpStatusCode.BadRequest, customApiException.ReturnHttpStatusCode);
        }

        [Fact(Skip = "This function used during download test")]
        public string WhenOperationCompleted_FileShoulBeExistWithNameofToken()
        {
            var generateProjectOverGitRequest = new GenerateProjectOverGitRequest()
                                                {
                                                    RepositoryLink = validGitRepoAddress,
                                                    RenamePairs = new List<KeyValuePair<string, string>>(),
                                                    BranchName = "master"
                                                };
            GenerateProjectResponse response = _sut.GenerateOverGit(generateProjectOverGitRequest);

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
                             new object[] {null},
                             new object[] {new GenerateProjectOverGitRequest()},
                             new object[] {new GenerateProjectOverGitRequest {BranchName = "master", RenamePairs = new List<KeyValuePair<string, string>>()}},
                             new object[] {new GenerateProjectOverGitRequest {BranchName = "master", RepositoryLink = validGitRepoAddress}},
                             new object[] {new GenerateProjectOverGitRequest {RenamePairs = new List<KeyValuePair<string, string>>(), RepositoryLink = validGitRepoAddress}},
                         };

            return result;
        }
    }
}