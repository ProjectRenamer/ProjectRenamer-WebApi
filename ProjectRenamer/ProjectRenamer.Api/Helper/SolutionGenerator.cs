using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
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
