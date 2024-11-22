namespace ConsoleApplication.model
{
    /**
     * <summary>
     * Class <c>Folder</c> corresponds to a Folder occurence in a folder structure.
     * This class contains standard information about folder's name, path and children related information such as nested files, folders and postfixes present in all subdirectories.
     * </summary>
     */
    public class Folder
    {
        public List<FileObject> NestedFiles{ get; set; }
        public List<Folder> NestedFolders { get; set; }
        public List<Postfix> NestedPostfixes { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Folder(string name, string path) {

            this.NestedFiles = new List<FileObject>();
            this.NestedFolders = new List<Folder>();
            this.NestedPostfixes = new List<Postfix>();
            this.Name = name;
            this.Path = path;
        }

        public Folder() { }
    }
}
