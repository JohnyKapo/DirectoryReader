using ConsoleApplication.model;
using ConsoleApplication.util.logger;

namespace ConsoleApplication.service
{
    /**
      * <summary>Service transforming real directory system to a <see cref="Folder"/> object.</summary>
      */
    public class LoadingService
    {
        private static Logger logger = Logger.GetInstance();

        /**
         * <summary>Function loads up directory content and returns it as a object.</summary>
         * <returns><see cref="Folder"/> object containing directory content.</returns>
         */
        public Folder? LoadUpFolderContent(string folderPath)
        {
            logger.LogDebug($"Loading up folder content of a folder with path: {folderPath}");
            try
            {
                return TraverseFolderContent(folderPath);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occurred while trying to load information about folder structure.", ex);
                return null;
            }
        }

        /**
         * <summary>Function traverses folder content with specified path.</summary>
         * <param name="folderPath">Path of a existing folder.</param>
         * <returns><see cref="Folder"/> object containing folder content.</returns>
         */
        private Folder TraverseFolderContent(string folderPath)
        {
            logger.LogDebug($"Traversing content of a folder with path: {folderPath}");
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
            logger.LogDebug($"Traversed content of a folder with path: ${folderPath}");
            return folder;
        }

        /**
         * <summary>Function goes through folders in parent folder.</summary>
         * <param name="folderPath">Path of a parent folder.</param>
         * <param name="folder"><see cref="Folder"/> object that is parent folder.</param>
         */
        private void TraverseFolderContentInFolder(string folderPath, Folder folder)
        {
            logger.LogDebug($"Traversing directories present in a folder with path: {folderPath}");
            string[] nestedFolders = Directory.GetDirectories(folderPath);
            foreach (string nestedFolder in nestedFolders)
            {
                Folder folder1 = TraverseFolderContent(nestedFolder);
                folder.NestedFolders.Add(folder1);
                AddPostfixesOfChildFolder(folder, folder1);  
            }
        }

        /**
         * <summary>Function passes postfixes from a child folder to the parent folder.</summary>
         * <param name="childFolder">Children folder.</param>
         * <param name="parentFolder">Parent folder.</param>
         */
        private void AddPostfixesOfChildFolder(Folder parentFolder, Folder childFolder)
        {
            logger.LogDebug("Passing available unique postfixes from child to parent folder.");
            foreach (Postfix postfix in childFolder.NestedPostfixes)
            {
                AddPostfix(parentFolder, postfix);
            }
        }

        /**
         * <summary>Function traverses files in parent folder.</summary>
         * <param name="folder">Parent folder.</param>
         * <param name="folderPath">Path of a parent folder.</param>
         */
        private void TraverseFileContentInFolder(string folderPath, Folder folder)
        {
            logger.LogDebug($"Traversing files present in a folder with path: {folderPath}");
            string[] nestedFiles = Directory.GetFiles(folderPath);
            foreach (string file in nestedFiles)
            {
                string fileName = file.Split('\\').Last();
                bool containsPostfix = fileName.Contains('.');
                string postfix = containsPostfix
                    // Check if file contains only leading period, if there are multiple period the last one will be postfix
                    ? fileName.Split('.').First().Equals(fileName.Split('.').Last()) ? "Without postfix" : fileName.Split('.').Last()
                    : "Without postfix";
                FileObject nestedFile = new FileObject(fileName, file, postfix);
                folder.NestedFiles.Add(nestedFile);
                Postfix filePostfix = new Postfix(postfix);
                AddPostfix(folder, filePostfix);
            }
        }

        /**
         * <summary>
         * Funtion adds postfix of a file to the parent folder.<br></br>
         * There two possible outcomes. It add to the parent folder postfix which was not yet found under this folder or the previous postfix will be incremented.
         * </summary>
         * <param name="folder">Parent folder.</param>
         * <param name="postfix"><see cref="Postfix"/> object.</param>
         */
        private async void AddPostfix(Folder folder, Postfix postfix)
        {
            if (!folder.NestedPostfixes.Select(p => p.PostfixVal).ToArray().Contains(postfix.PostfixVal))
            {
                logger.LogDebug($"Postfix of a type '{postfix.PostfixVal}' does not exist in a folder '{folder.Name}'. Creating a new postfix of this type for this folder.");
                Postfix newPostfix = new Postfix(postfix.PostfixVal, postfix.Count);
                folder.NestedPostfixes.Add(newPostfix);
            }
            else
            {
                logger.LogDebug($"Postfix of a type '{postfix.PostfixVal}' already exists in a folder '{folder.Name}'. Incrementing count if that postfix by '{postfix.Count}'.");
                Postfix existingPostfix = folder.NestedPostfixes.First(p => p.PostfixVal == postfix.PostfixVal);
                existingPostfix.Count += postfix.Count;
            }
        }
    }
}
