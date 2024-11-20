namespace ConsoleApplication.model
{
    public class Folder : AbstractDirectoryObject
    {
        public List<FileObject> NestedFiles{ get; set; }
        public List<Folder> NestedFolders { get; set; }

        public Folder(string name, string path) : base(name, path) {

            this.NestedFiles = new List<FileObject>();
            this.NestedFolders = new List<Folder>();
        }
    }
}
