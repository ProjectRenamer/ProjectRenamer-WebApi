using System;
using System.Collections.Generic;
using System.Net;
using Alternatives.CustomExceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ProjectRenamer.Api.Controllers;
using ProjectRenamer.Api.Requests;
using ProjectRenamer.Api.Test.Fixture;
using Xunit;

namespace ProjectRenamer.Api.Test.ProjectGeneratorControllerTest
{
    public class DownloadTest : IClassFixture<ValidationFixture>
    {
        private readonly ValidationFixture _validationFixture;
        private ProjectGeneratorController _sut;

        public DownloadTest(ValidationFixture validationFixture)
        {
            _validationFixture = validationFixture;
            _sut = new ProjectGeneratorController(_validationFixture.ValidatorResolver);
        }

        [Theory]
        [MemberData(nameof(InvalidRequests))]
        public void WhenRequestIsInvalid_CustomApiExceptionOccurs(DownloadProjectRequest downloadProjectRequest)
        {
            var customApiException = Assert.Throws<CustomApiException>(() => _sut.Download(downloadProjectRequest));
            Assert.Equal(HttpStatusCode.BadRequest, customApiException.ReturnHttpStatusCode);
        }

        [Fact]
        public void WhenTokenIsNotExist_CustomApiExceptionOccurs()
        {
            var downloadProjectRequest = new DownloadProjectRequest()
                                         {
                                             Token = Guid.NewGuid().ToString()
                                         };
            var customApiException = Assert.Throws<CustomApiException>(() => _sut.Download(downloadProjectRequest));
            Assert.Equal($"{downloadProjectRequest.Token} not valid", customApiException.FriendlyMessage);
        }

        [Fact]
        public void WhenTokenIsValid_ResponseShouldNotBeEmpty()
        {
            var generateTest = new GenerateTest(_validationFixture);
            string token = generateTest.WhenOperationCompleted_FileShoulBeExistWithNameofToken();

            var downloadProjectRequest = new DownloadProjectRequest()
                                         {
                                             Token = token
                                         };

            FileContentResult fileContentResult = _sut.Download(downloadProjectRequest);

            Assert.NotNull(fileContentResult);
            Assert.Equal(token, fileContentResult.FileDownloadName);
        }

        public static List<object[]> InvalidRequests()
        {
            var result = new List<object[]>
                         {
                             new object[] {null},
                             new object[] {new DownloadProjectRequest()}
                         };

            return result;
        }
    }
}