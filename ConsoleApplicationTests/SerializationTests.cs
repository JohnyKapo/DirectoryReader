using ConsoleApplication.model;
using ConsoleApplication.serialization;

namespace ConsoleApplicationTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Serialize_InputIsNull()
        {
            CustomJsonSerializer serializer = new CustomJsonSerializer();
            string json = serializer.Serialize(null);
            Assert.AreEqual("null", json);
        }

        [TestMethod]
        public void Serialize_InputIsSingleLevelFolder()
        {
            CustomJsonSerializer serializer = new CustomJsonSerializer();
            string json = serializer.Serialize(new Folder("folder", "./folder"));
            Assert.IsNotNull(json);
            Assert.AreEqual(
                """{"NestedFiles":[],"NestedFolders":[],"NestedPostfixes":[],"Name":"folder","Path":"./folder"}""",
                json
                );
        }

        [TestMethod]
        public void Serialize_MoreComplexFolderStructure()
        {
            /**
             * Structure:
             * [level-1-folder]
             *      - [level-2-folder1]
             *          - [level-3-folder]
             *              - {level-4-file.yang}
             *      - [level-2-folder2]
             *          - [level-3-folder]
             *              - [level-4-folder]
             *          - {level-3-file.xml}
             *      - {level-2-file1.json}
             *      - {level-2-file2.yaml}
             */

            CustomJsonSerializer serializer = new CustomJsonSerializer();

            // Level 1
            Folder folder = new Folder("level-1-folder", "./level-1-folder");
            folder.NestedPostfixes.Add(new Postfix("yang"));
            folder.NestedPostfixes.Add(new Postfix("json"));
            folder.NestedPostfixes.Add(new Postfix("yaml"));
            folder.NestedPostfixes.Add(new Postfix("xml"));

            // Level 2
            Folder level2Folder1 = new Folder("level-2-folder1", "./level-1-folder/level-2-folder1");
            Folder level2Folder2 = new Folder("level-2-folder2", "./level-1-folder/level-2-folder2");
            FileObject level2File1 = new FileObject("level-2-file1.json", "./level-1-folder/level-2-file1.json", "json");
            FileObject level2File2 = new FileObject("level-2-file2.yaml", "./level-1-folder/level-2-file2.yaml", "yaml");
            folder.NestedFolders.Add(level2Folder1);
            folder.NestedFolders.Add(level2Folder2);
            folder.NestedFiles.Add(level2File1);
            folder.NestedFiles.Add(level2File2);
            level2Folder1.NestedPostfixes.Add(new Postfix("yang"));
            level2Folder2.NestedPostfixes.Add(new Postfix("xml"));

            // Level 3
            Folder level3folder1 = new Folder("level-3-folder", "./level-1-folder/level-2-folder1/level-3-folder");
            Folder level3folder2 = new Folder("level-3-folder", "./level-1-folder/level-2-folder2/level-3-folder");
            FileObject level3File = new FileObject("level-3-file.xml", "./level-1-folder/level-2-folder2/level-3-file.xml", "xml");
            level2Folder1.NestedFolders.Add(level3folder1);
            level2Folder2.NestedFolders.Add(level3folder2);
            level2Folder2.NestedFiles.Add(level3File);

            // Level 4
            FileObject level4File = new FileObject("level-4-file.yang", "./level-1-folder/level-2-folder1/level-3-folder/level-4-file.yang", "yang");
            Folder level4Folder = new Folder("level-4-folder", "./level-1-folder/level-2-folder2/level-3-folder/level-4-folder");
            level3folder1.NestedFiles.Add(level4File);
            level3folder2.NestedFolders.Add(level4Folder);
            level3folder1.NestedPostfixes.Add(new Postfix("yang"));

            string json = serializer.Serialize(folder);
            Assert.IsNotNull(json);
            Assert.AreEqual(
                """{"NestedFiles":[{"Postfix":"json","Name":"level-2-file1.json","Path":"./level-1-folder/level-2-file1.json"},{"Postfix":"yaml","Name":"level-2-file2.yaml","Path":"./level-1-folder/level-2-file2.yaml"}],"NestedFolders":[{"NestedFiles":[],"NestedFolders":[{"NestedFiles":[{"Postfix":"yang","Name":"level-4-file.yang","Path":"./level-1-folder/level-2-folder1/level-3-folder/level-4-file.yang"}],"NestedFolders":[],"NestedPostfixes":[{"PostfixVal":"yang","Count":1}],"Name":"level-3-folder","Path":"./level-1-folder/level-2-folder1/level-3-folder"}],"NestedPostfixes":[{"PostfixVal":"yang","Count":1}],"Name":"level-2-folder1","Path":"./level-1-folder/level-2-folder1"},{"NestedFiles":[{"Postfix":"xml","Name":"level-3-file.xml","Path":"./level-1-folder/level-2-folder2/level-3-file.xml"}],"NestedFolders":[{"NestedFiles":[],"NestedFolders":[{"NestedFiles":[],"NestedFolders":[],"NestedPostfixes":[],"Name":"level-4-folder","Path":"./level-1-folder/level-2-folder2/level-3-folder/level-4-folder"}],"NestedPostfixes":[],"Name":"level-3-folder","Path":"./level-1-folder/level-2-folder2/level-3-folder"}],"NestedPostfixes":[{"PostfixVal":"xml","Count":1}],"Name":"level-2-folder2","Path":"./level-1-folder/level-2-folder2"}],"NestedPostfixes":[{"PostfixVal":"yang","Count":1},{"PostfixVal":"json","Count":1},{"PostfixVal":"yaml","Count":1},{"PostfixVal":"xml","Count":1}],"Name":"level-1-folder","Path":"./level-1-folder"}""",
                json
                );
        }

        [TestMethod]
        public void Deserialize_JsonIsNull() 
        { 
            CustomJsonSerializer serializer = new CustomJsonSerializer();
            Folder folder = serializer.Deserialize(null);
            Assert.IsNull(folder);
        }

        [TestMethod]
        public void Deserialize_JsonIsEmpty()
        {
            CustomJsonSerializer serializer = new CustomJsonSerializer();
            Folder folder = serializer.Deserialize("");
            Assert.IsNull(folder);
        }

        [TestMethod]
        public void Deserialize_JsonIsInvalid()
        {
            CustomJsonSerializer serializer = new CustomJsonSerializer();
            Folder folder = serializer.Deserialize("""{"invalidToken":"null","invalidList":[]}""");
            Assert.IsNotNull(folder);
            Assert.IsNull(folder.Name);
            Assert.IsNull(folder.Path);
            Assert.IsNull(folder.NestedFolders);
            Assert.IsNull(folder.NestedFiles);
            Assert.IsNull(folder.NestedPostfixes);    
        }

        [TestMethod]
        public void Deserialize_JsonHasValidContent()
        {
            /**
             * Structure:
             * [level-1-folder]
             *      - [level-2-folder1]
             *          - [level-3-folder]
             *              - {level-4-file.yang}
             *      - [level-2-folder2]
             *          - [level-3-folder]
             *              - [level-4-folder]
             *          - {level-3-file.xml}
             *      - {level-2-file1.json}
             *      - {level-2-file2.yaml}
             */
            CustomJsonSerializer serializer = new CustomJsonSerializer();
            Folder folder = serializer.Deserialize("""{"NestedFiles":[{"Postfix":"json","Name":"level-2-file1.json","Path":"./level-1-folder/level-2-file1.json"},{"Postfix":"yaml","Name":"level-2-file2.yaml","Path":"./level-1-folder/level-2-file2.yaml"}],"NestedFolders":[{"NestedFiles":[],"NestedFolders":[{"NestedFiles":[{"Postfix":"yang","Name":"level-4-file.yang","Path":"./level-1-folder/level-2-folder1/level-3-folder/level-4-file.yang"}],"NestedFolders":[],"NestedPostfixes":[{"PostfixVal":"yang","Count":1}],"Name":"level-3-folder","Path":"./level-1-folder/level-2-folder1/level-3-folder"}],"NestedPostfixes":[{"PostfixVal":"yang","Count":1}],"Name":"level-2-folder1","Path":"./level-1-folder/level-2-folder1"},{"NestedFiles":[{"Postfix":"xml","Name":"level-3-file.xml","Path":"./level-1-folder/level-2-folder2/level-3-file.xml"}],"NestedFolders":[{"NestedFiles":[],"NestedFolders":[{"NestedFiles":[],"NestedFolders":[],"NestedPostfixes":[],"Name":"level-4-folder","Path":"./level-1-folder/level-2-folder2/level-3-folder/level-4-folder"}],"NestedPostfixes":[],"Name":"level-3-folder","Path":"./level-1-folder/level-2-folder2/level-3-folder"}],"NestedPostfixes":[{"PostfixVal":"xml","Count":1}],"Name":"level-2-folder2","Path":"./level-1-folder/level-2-folder2"}],"NestedPostfixes":[{"PostfixVal":"yang","Count":1},{"PostfixVal":"json","Count":1},{"PostfixVal":"yaml","Count":1},{"PostfixVal":"xml","Count":1}],"Name":"level-1-folder","Path":"./level-1-folder"}""");

            // Level 1
            Assert.AreEqual("level-1-folder", folder.Name);
            Assert.AreEqual("./level-1-folder", folder.Path);
            Assert.AreEqual(2, folder.NestedFolders.Count);
            Assert.AreEqual(2, folder.NestedFiles.Count);
            Assert.AreEqual(4, folder.NestedPostfixes.Count);

            // Level 2
            Folder level2Folder1 = folder.NestedFolders.First(f => f.Name.Equals("level-2-folder1"));
            Folder level2Folder2 = folder.NestedFolders.First(f => f.Name.Equals("level-2-folder2"));
            FileObject level2File1 = folder.NestedFiles.First(f => f.Name.Equals("level-2-file1.json"));
            FileObject level2File2 = folder.NestedFiles.First(f => f.Name.Equals("level-2-file2.yaml"));
            Assert.AreEqual("level-2-folder1", level2Folder1.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder1", level2Folder1.Path);
            Assert.AreEqual(1, level2Folder1.NestedFolders.Count);
            Assert.AreEqual(0, level2Folder1.NestedFiles.Count);
            Assert.AreEqual(1, level2Folder1.NestedPostfixes.Count);
            Assert.AreEqual("level-2-folder2", level2Folder2.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder2", level2Folder2.Path);
            Assert.AreEqual(1, level2Folder2.NestedFolders.Count);
            Assert.AreEqual(1, level2Folder2.NestedFiles.Count);
            Assert.AreEqual(1, level2Folder2.NestedPostfixes.Count);
            Assert.AreEqual("level-2-file1.json", level2File1.Name);
            Assert.AreEqual("./level-1-folder/level-2-file1.json", level2File1.Path);
            Assert.AreEqual("json", level2File1.Postfix);
            Assert.AreEqual("level-2-file2.yaml", level2File2.Name);
            Assert.AreEqual("./level-1-folder/level-2-file2.yaml", level2File2.Path);
            Assert.AreEqual("yaml", level2File2.Postfix);

            // Level 3
            Folder level3folder1 = level2Folder1.NestedFolders.First(f => f.Name.Equals("level-3-folder"));
            Folder level3folder2 = level2Folder2.NestedFolders.First(f => f.Name.Equals("level-3-folder"));
            FileObject level3File = level2Folder2.NestedFiles.First();
            Assert.AreEqual("level-3-folder", level3folder1.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder1/level-3-folder", level3folder1.Path);
            Assert.AreEqual(0, level3folder1.NestedFolders.Count);
            Assert.AreEqual(1, level3folder1.NestedFiles.Count);
            Assert.AreEqual(1, level3folder1.NestedPostfixes.Count);
            Assert.AreEqual("level-3-folder", level3folder1.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder2/level-3-folder", level3folder2.Path);
            Assert.AreEqual(1, level3folder2.NestedFolders.Count);
            Assert.AreEqual(0, level3folder2.NestedFiles.Count);
            Assert.AreEqual(0, level3folder2.NestedPostfixes.Count);
            Assert.AreEqual("level-3-file.xml", level3File.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder2/level-3-file.xml", level3File.Path);
            Assert.AreEqual("xml", level3File.Postfix);

            // Level 4
            FileObject level4File = level3folder1.NestedFiles.First();
            Folder level4Folder = level3folder2.NestedFolders.First();
            Assert.AreEqual("level-4-folder", level4Folder.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder2/level-3-folder/level-4-folder", level4Folder.Path);
            Assert.AreEqual(0, level4Folder.NestedFolders.Count);
            Assert.AreEqual(0, level4Folder.NestedFiles.Count);
            Assert.AreEqual(0, level4Folder.NestedPostfixes.Count);
            Assert.AreEqual("level-4-file.yang", level4File.Name);
            Assert.AreEqual("./level-1-folder/level-2-folder1/level-3-folder/level-4-file.yang", level4File.Path);
            Assert.AreEqual("yang", level4File.Postfix);
        }

    }
}
