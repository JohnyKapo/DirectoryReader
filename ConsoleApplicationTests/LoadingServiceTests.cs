using ConsoleApplication.model;
using ConsoleApplication.service;
using System.Runtime.CompilerServices;

namespace ConsoleApplicationTests
{
    [TestClass]
    public class LoadingServiceTests
    {
        private string GetTestAssemblyDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location);
        }

        private string GetResourcesFolder()
        {
            string testAssemblyDir = GetTestAssemblyDirectory();
            return Path.Combine(testAssemblyDir, "..", "..", "..", "resources");
        }

        [TestMethod]
        public void LoadUpFolderContent_PathIsNull()
        {
            LoadingService service = new LoadingService();
            Assert.IsNull(service.LoadUpFolderContent(null));
        }

        [TestMethod]
        public void LoadUpFolderContent_PathIsInvalid()
        {
            LoadingService service = new LoadingService();
            Assert.IsNull(service.LoadUpFolderContent("XXX:\\Invalid\\Path\\To\\Folder"));
        }

        [TestMethod]
        public void LoadUpFolderContent_InputPathIsAFile()
        {
            LoadingService service = new LoadingService();
            Assert.IsNull(service.LoadUpFolderContent(Path.Combine(GetResourcesFolder(), "LoadUpFolderContent_InputPathIsAFile", "level-1-file.json")));
        }

        /**
         * When tested via github actions, specific folder must be created before testing [level-1-folder]
         */
        [TestMethod]
        public void LoadUpFolderContent_SingleLevelFolder()
        {
            LoadingService service = new LoadingService();
            Folder folder = service.LoadUpFolderContent(Path.Combine(GetResourcesFolder(), "LoadUpFolderContent_SingleLevelFolder", "level-1-folder"));
            Assert.IsNotNull(folder);
            Assert.AreEqual("level-1-folder", folder.Name);
            Assert.IsTrue(folder.NestedFolders.Count == 0);
            Assert.IsTrue(folder.NestedFiles.Count == 0);
            Assert.IsTrue(folder.NestedPostfixes.Count == 0);
        }

        /**
         * When tested via github actions, specific folder must be created before testing [level-1-folder\level-2-folder]
         */
        [TestMethod]
        public void LoadUpFolderContent_ContainsOneFolderAndOneFile()
        {
            LoadingService service = new LoadingService();
            Folder folder = service.LoadUpFolderContent(Path.Combine(GetResourcesFolder(), "LoadUpFolderContent_ContainsOneFolderAndOneFile", "level-1-folder"));
            Assert.IsNotNull(folder);
            Assert.AreEqual("level-1-folder", folder.Name);
            Assert.IsTrue(folder.NestedFolders.Count == 1);
            Assert.IsTrue(folder.NestedFiles.Count == 1);
            Assert.IsTrue(folder.NestedPostfixes.Count == 1);

            // Check nested folder
            Folder nestedFolder = folder.NestedFolders.First();
            Assert.AreEqual("level-2-folder", nestedFolder.Name);
            Assert.IsTrue(nestedFolder.NestedFolders.Count == 0);
            Assert.IsTrue(nestedFolder.NestedFiles.Count == 0);
            Assert.IsTrue(nestedFolder.NestedPostfixes.Count == 0);
        
            // Check nested file
            FileObject nestedFile = folder.NestedFiles.First();
            Assert.AreEqual("level-2-file.json", nestedFile.Name);
            Assert.AreEqual("json", nestedFile.Postfix);

            // Check postfix values
            Postfix postfix = folder.NestedPostfixes.First();
            Assert.AreEqual("json", postfix.PostfixVal);
            Assert.AreEqual(1, postfix.Count);
        }

        [TestMethod]
        public void LoadUpFolderContent_ContainsMultiplePostfixTypes()
        {
            LoadingService service = new LoadingService();
            Folder folder = service.LoadUpFolderContent(Path.Combine(GetResourcesFolder(), "LoadUpFolderContent_ContainsMultiplePostfixTypes", "level-1-folder"));
            Assert.IsNotNull(folder);
            Assert.AreEqual("level-1-folder", folder.Name);
            Assert.IsTrue(folder.NestedFolders.Count == 1);
            Assert.IsTrue(folder.NestedFiles.Count == 2);
            Assert.IsTrue(folder.NestedPostfixes.Count == 4);

            // Check all postfixes
            List<Postfix> listOfPostfixes = folder.NestedPostfixes.ToList();

            Postfix jsonPostfix = listOfPostfixes.First(f => f.PostfixVal.Equals("json"));
            Assert.IsNotNull(jsonPostfix);
            Assert.AreEqual(2, jsonPostfix.Count);

            Postfix yamlPostfix = listOfPostfixes.First(f => f.PostfixVal.Equals("yaml"));
            Assert.IsNotNull(yamlPostfix);
            Assert.AreEqual(1, yamlPostfix.Count);

            Postfix xmlPostfix = listOfPostfixes.First(f => f.PostfixVal.Equals("xml"));
            Assert.IsNotNull(xmlPostfix);
            Assert.AreEqual(1, xmlPostfix.Count);

            Postfix withoutPostfix = listOfPostfixes.First(f => f.PostfixVal.Equals("Without postfix"));
            Assert.IsNotNull(withoutPostfix);
            Assert.AreEqual(1, withoutPostfix.Count);
        }

        [TestMethod]
        public void LoadUpFolderContent_FileContainsMultiplePeriods()
        {
            LoadingService service = new LoadingService();
            Folder folder = service.LoadUpFolderContent(Path.Combine(GetResourcesFolder(), "LoadUpFolderContent_FileContainsMultiplePeriods", "level-1-folder"));
            Assert.IsNotNull(folder);
            Assert.AreEqual("level-1-folder", folder.Name);
            Assert.IsTrue(folder.NestedFolders.Count == 0);
            Assert.IsTrue(folder.NestedFiles.Count == 1);
            Assert.IsTrue(folder.NestedPostfixes.Count == 1);

            // Check nested file
            FileObject nestedFile = folder.NestedFiles.First();
            Assert.IsNotNull(nestedFile);
            Assert.AreEqual("level-2-file.cs.json.xml", nestedFile.Name);
            string postfix = nestedFile.Postfix;
            Assert.IsNotNull(postfix);
            Assert.AreEqual("xml", postfix);
        }
    }
}
