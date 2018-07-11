using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Alternatives.CustomExceptions;

namespace ProjectRenamer.Api.Helper
{
    public class SolutionRenamer
    {
        private readonly string[] _skipFolders
            = new[]
              {
                  ".git"
              };

        public void Run(string folder, List<KeyValuePair<string, string>> renamePairs)
        {
            if (!Directory.Exists(folder))
            {
                throw new Exception("There is no folder: " + folder);
            }

            folder = folder.Trim('\\');

            if (renamePairs == null)
            {
                throw new CustomApiException("RenamePairs cannot be null", HttpStatusCode.InternalServerError);
            }

            foreach (var pair in renamePairs)
            {
                ClearHiddenGitDirectory(folder);
                RenameDirectoryRecursively(folder, pair.Key, pair.Value);
                RenameAllFiles(folder, pair.Key, pair.Value);
                ReplaceContent(folder, pair.Key, pair.Value);
            }
        }


        private void ClearHiddenGitDirectory(string folder)
        {
            FileHelper.DeleteDirectory($"{folder}/.git");
        }

        private void RenameDirectoryRecursively(string directoryPath, string placeHolder, string name)
        {
            string[] subDirectories = Directory.GetDirectories(directoryPath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string subDirectory in subDirectories)
            {
                string newDir = subDirectory;
                if (subDirectory.Contains(placeHolder))
                {
                    newDir = subDirectory.Replace(placeHolder, name);
                    Directory.Move(subDirectory, newDir);
                }

                RenameDirectoryRecursively(newDir, placeHolder, name);
            }
        }


        private void RenameAllFiles(string directory, string placeHolder, string name)
        {
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (_skipFolders.Any(x => file.Contains(x)))
                {
                    continue;
                }

                if (file.Contains(placeHolder))
                {
                    File.Move(file, file.Replace(placeHolder, name));
                }
            }
        }


        private void ReplaceContent(string rootPath, string placeHolder, string name)
        {
            var skipExtensions = new[]
                                 {
                                     ".exe",
                                     ".dll",
                                     ".bin",
                                     ".suo",
                                     ".png",
                                     "jpg",
                                     "jpeg",
                                     ".pdb",
                                     ".obj"
                                 };

            string[] files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (_skipFolders.Any(x => file.Contains(x)))
                {
                    continue;
                }

                if (skipExtensions.Contains(Path.GetExtension(file)))
                {
                    continue;
                }

                long fileSize = GetFileSize(file);
                if (fileSize < placeHolder.Length)
                {
                    continue;
                }

                Encoding encoding = GetEncoding(file);

                string content = File.ReadAllText(file, encoding);
                string newContent = content.Replace(placeHolder, name);
                if (newContent != content)
                {
                    File.WriteAllText(file, newContent, encoding);
                }
            }
        }

        private long GetFileSize(string file)
        {
            return new FileInfo(file).Length;
        }

        private Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
            {
                return Encoding.UTF7;
            }
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            {
                return Encoding.UTF8;
            }
            if (bom[0] == 0xff && bom[1] == 0xfe)
            {
                return Encoding.Unicode; //UTF-16LE
            }
            if (bom[0] == 0xfe && bom[1] == 0xff)
            {
                return Encoding.BigEndianUnicode; //UTF-16BE
            }
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
            {
                return Encoding.UTF32;
            }

            return Encoding.ASCII;
        }
    }
}