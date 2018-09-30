using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using Alternatives;
using Alternatives.CustomExceptions;
using LibGit2Sharp;
using Microsoft.AspNetCore.Http;

namespace ProjectRenamer.Api.Helper
{
    public class SolutionGenerator
    {
        private readonly DirectoryInfo _directory = Directory.CreateDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/temp");

        public string DownloadRepoFromGit(string repositoryLink, string branchName, string userName, string pass)
        {
            string fileName = $"{Guid.NewGuid():N}";
            string templatePath = Path.Combine(_directory.FullName, fileName);

            var cloneOptions = new CloneOptions
                               {
                                   BranchName = branchName,
                                   CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                                                                                 {
                                                                                     Username = userName,
                                                                                     Password = pass
                                                                                 }
                               };

            try
            {
                string clonedRepoPath = Repository.Clone(repositoryLink, templatePath, cloneOptions);
                new Repository(clonedRepoPath).Dispose();
            }
            catch (System.IO.PathTooLongException ex)
            {
                FileHelper.DeleteDirectory(templatePath);
                throw new CustomApiException("File names too long", HttpStatusCode.BadRequest, ex);
            }
            catch (LibGit2SharpException ex)
            {
                FileHelper.DeleteDirectory(templatePath);
                throw new CustomApiException(ex.Message, HttpStatusCode.BadRequest);
            }

            return fileName;
        }

        public string Generate(string fileName, List<KeyValuePair<string, string>> renamePairs)
        {
            string templatePath = Path.Combine(_directory.FullName, fileName);

            var solutionRenamer = new SolutionRenamer();
            solutionRenamer.Run(templatePath, renamePairs);

            return fileName;
        }

        public string Upload(IFormFile file)
        {
            Guard.IsTrue(file == null || file.Length == 0, new CustomApiException("File cannot found", HttpStatusCode.BadRequest));

            string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);

            string fileName = $"{Guid.NewGuid():N}";
            string templatePath = Path.Combine(_directory.FullName, fileName);
            string templateZipPath = templatePath + $".{fileExtension}";

            using (var stream = new FileStream(templateZipPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            ZipFile.ExtractToDirectory(templateZipPath, templatePath);
            File.Delete(templateZipPath);

            return fileName;
        }

        public byte[] Download(string token)
        {
            string templatePath = Path.Combine(_directory.FullName, token);

            if (!Directory.Exists(templatePath))
            {
                throw new CustomApiException($"{token} not valid", HttpStatusCode.NotFound);
            }

            string zipPath = Path.Combine(_directory.FullName, $"{token}.zip");
            ZipFile.CreateFromDirectory(templatePath, zipPath);

            byte[] zipBytes = System.IO.File.ReadAllBytes(zipPath);

            System.IO.File.Delete(zipPath);
            FileHelper.DeleteDirectory(templatePath);

            return zipBytes;
        }
    }
}