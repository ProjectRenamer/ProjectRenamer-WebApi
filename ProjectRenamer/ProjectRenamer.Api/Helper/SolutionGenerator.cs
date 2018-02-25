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
        public byte[] Generate(string repositoryLink, string projectName, List<KeyValuePair<string, string>> renamePairs, CloneOptions cloneOptions)
        {
            DirectoryInfo directory = Directory.GetParent(Directory.GetCurrentDirectory());

            string templatePath = Path.Combine(directory.FullName, $"template-{Guid.NewGuid():N}");
            string zipPath = Path.Combine(directory.FullName, $"{projectName}.zip");
            try
            {
                string clonedRepoPath = Repository.Clone(repositoryLink, templatePath, cloneOptions);

                new Repository(clonedRepoPath).Dispose();

                var solutionRenamer = new SolutionRenamer();
                solutionRenamer.Run(templatePath, renamePairs);

                ZipFile.CreateFromDirectory(templatePath, zipPath);
            }
            catch (System.IO.PathTooLongException ex)
            {
                throw new CustomApiException("File names too long", HttpStatusCode.BadRequest, ex);
            }
            catch(LibGit2SharpException ex)
            {
                throw new CustomApiException(ex.Message, HttpStatusCode.BadRequest);
            }
            finally
            {
                FileHelper.DeleteDirectory(templatePath);
            }

            byte[] zipBytes = System.IO.File.ReadAllBytes(zipPath);

            System.IO.File.Delete(zipPath);

            return zipBytes;
        }
    }
}
