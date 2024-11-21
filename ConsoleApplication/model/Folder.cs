namespace ConsoleApplication.model
{
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
