namespace ConsoleApplication.model
{
    public class FileObject : AbstractDirectoryObject
    {
        public string Postfix { get; set; }

        public FileObject(string name, string path, string postfix) : base(name, path)
        {
            this.Postfix = postfix;
        }

    }
}
