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
                throw new FileLoadException("Parent folder with path '" + folderPath + "' does not exist.");
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
                string postfix = file.Split('.').Last();
                string fileName = file.Split('\\').Last();
                FileObject nestedFile = new FileObject(fileName, file, postfix);
                folder.NestedFiles.Add(nestedFile);
            }
        }
    }
}
