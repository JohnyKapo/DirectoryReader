using ConsoleApplication.model;

namespace ConsoleApplication.service
{
    public class LoadingService
    {
        public Folder LoadUpFolderContent(string folderPath)
        {
            return TraverseFolderContent(folderPath);
        }

        private Folder TraverseFolderContent(string folderPath)
        {
            // Check if folder with specified path exists
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException("Parent folder with path '" + folderPath + "' does not exist.");
            }
            // Create new folder object
            string folderName = folderPath.Split('\\').Last();
            Folder folder = new Folder(folderName, folderPath);
            TraverseFileContentInFolder(folderPath, folder);
            TraverseFolderContentInFolder(folderPath, folder);
            return folder;
        }

        private void TraverseFolderContentInFolder(string folderPath, Folder folder)
        {
            string[] nestedFolders;
            try
            {
                nestedFolders = Directory.GetDirectories(folderPath);
            }
            catch (Exception e)
            {
                // add log
                return;
            }

            foreach (string nestedFolder in nestedFolders)
            {
                Folder folder1 = TraverseFolderContent(nestedFolder);
                folder.NestedFolders.Add(folder1);
                AddPostfixesOfChildFolder(folder, folder1);  
            }
        }

        private void AddPostfixesOfChildFolder(Folder parentFolder, Folder childFolder)
        {
            foreach (Postfix postfix in childFolder.NestedPostfixes)
            {
                AddPostfix(parentFolder, postfix);
            }
        }

        private void TraverseFileContentInFolder(string folderPath, Folder folder)
        {
            string[] nestedFiles;
            try
            {
                nestedFiles = Directory.GetFiles(folderPath);
            }
            catch (Exception e)
            {
                // add log
                return;
            }

            foreach (string file in nestedFiles)
            {
                string fileName = file.Split('\\').Last();
                bool containsPostfix = fileName.Contains('.');
                string postfix = containsPostfix ? fileName.Split('.').Last() : "Without postfix";
                FileObject nestedFile = new FileObject(fileName, file, postfix);
                folder.NestedFiles.Add(nestedFile);
                Postfix filePostfix = new Postfix(postfix);
                AddPostfix(folder, filePostfix);
            }
        }

        private void AddPostfix(Folder folder, Postfix postfix)
        {
            if (!folder.NestedPostfixes.Select(p => p.PostfixVal).ToArray().Contains(postfix.PostfixVal))
            {
                Postfix newPostfix = new Postfix(postfix.PostfixVal, postfix.Count);
                folder.NestedPostfixes.Add(newPostfix);
            }
            else
            {
                Postfix existingPostfix = folder.NestedPostfixes.First(p => p.PostfixVal == postfix.PostfixVal);
                existingPostfix.Count += postfix.Count;
            }
        }
    }
}
