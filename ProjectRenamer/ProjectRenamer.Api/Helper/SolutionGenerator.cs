using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alternatives.CustomExceptions;
using LibGit2Sharp;

namespace ProjectRenamer.Api.Helper
{
    public class SolutionGenerator
    {
        DirectoryInfo directory = Directory.GetParent(Directory.GetCurrentDirectory());

        public string Generate(string repositoryLink, string projectName, List<KeyValuePair<string, string>> renamePairs, CloneOptions cloneOptions)
        {
            string fileName = $"template-{Guid.NewGuid():N}";
            string templatePath = Path.Combine(directory.FullName, fileName);
            try
            {
                string clonedRepoPath = Repository.Clone(repositoryLink, templatePath, cloneOptions);

                new Repository(clonedRepoPath).Dispose();

                var solutionRenamer = new SolutionRenamer();
                solutionRenamer.Run(templatePath, renamePairs);
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

        public byte[] Download(string fileName)
        {
            string templatePath = Path.Combine(directory.FullName, fileName);

            if (!File.Exists(templatePath))
            {
                throw new CustomApiException($"{fileName} not valid", HttpStatusCode.NotFound);
            }

            string zipPath = Path.Combine(directory.FullName, $"{fileName}.zip");
            ZipFile.CreateFromDirectory(templatePath, zipPath);

            byte[] zipBytes = System.IO.File.ReadAllBytes(zipPath);

            System.IO.File.Delete(zipPath);
            FileHelper.DeleteDirectory(templatePath);

            return zipBytes;
        }
    }
}
