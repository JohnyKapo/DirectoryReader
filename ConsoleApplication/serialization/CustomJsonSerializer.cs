using ConsoleApplication.model;
using System.Text.Json;

namespace ConsoleApplication.serialization
{
    public class CustomJsonSerializer
    {
        public string Serialize(Folder folder)
        {
            try
            {
                return JsonSerializer.Serialize(folder);
            }
            catch (NotSupportedException e)
            {
                // log
                return null;
            }
        }

        public Folder Deserialize(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Folder>(json);
            }
            catch (Exception e)
            {
                // log
                return null;
            }
        }

        public async void SerializeToFile(Folder folder, string path)
        {
            if (folder == null || path == null)
            {
                return;
            }

            try
            {
                if (!File.Exists(path))
                {
                    using FileStream fs = File.Create(path);
                }
                else
                {
                    Console.WriteLine("File already exists. The content will be rewritten.");
                }

                string json = Serialize(folder);

                using StreamWriter outputFile = new StreamWriter(path);
                await outputFile.WriteAsync(json);
            }
            catch
            {
                // log
            }
        }

        public async Task<Folder> DeserializeFolderStructureFromFile(string path)
        {
            try
            {
                using StreamReader inputFile = new StreamReader(path);
                return Deserialize(await inputFile.ReadToEndAsync());
            }
            catch
            {
                return null;
            }
        }
    }

}
