namespace ConsoleApplication.model
{
    /**
     * <summary>
     * Class <c>FileObject</c> corresponds to a File occurence in folder structure.
     * This class contains information about file's name, path and postfix.
     * </summary>
     */
    public class FileObject
    {
        public string Postfix { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public FileObject(string name, string path, string postfix)
        {
            this.Postfix = postfix;
            this.Name = name;
            this.Path = path;
        }

        public FileObject() { } 
    }
}
