using ConsoleApplication.model;
using ConsoleApplication.util.logger;
using System.Text.Json;

namespace ConsoleApplication.serialization
{
    /**
     * <summary>
     * Custom serializer for this project.
     * <para>Contains funcionalities such as standard de/serializing and de/serializing to/from file.</para>
     * </summary>
     */
    public class CustomJsonSerializer
    {
        private static Logger logger = Logger.GetInstance();

        /**
         * <summary>Serializes <see cref="Folder"/> object to JSON string.</summary>
         * <param name="folder">Given folder structure.</param>
         */
        public string? Serialize(Folder folder)
        {
            try
            {
                return JsonSerializer.Serialize(folder);
            }
            catch (Exception ex)
            {
                logger.LogError("Serializer was not able to serialize input.", ex);
                return null;
            }
        }

        /**
         * <summary>Deserializes string json to a <see cref="Folder"/> object.</summary>
         * <param name="json">JSON of a string type.</param>
         */
        public Folder? Deserialize(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Folder>(json);
            }
            catch (Exception ex)
            {
                logger.LogError("Serializer was not able to deserialize input.", ex);
                return null;
            }
        }

        /**
         * <summary>Serializes <see cref="Folder"/> object to JSON string and then saves it to a file.</summary>
         * <param name="folder">Given folder structure.</param>
         * <param name="path">Path to which file should be saved.</param>
         */
        public async void SerializeToFile(Folder folder, string path)
        {
            if (folder == null || path == null)
            {
                logger.LogWarning("Serializer was not able to serialiaze Folder object to file. One of the parameters is null.");
                return;
            }

            try
            {
                if (!File.Exists(path))
                {
                    using FileStream fs = File.Create(path);
                }

                string json = Serialize(folder);

                using StreamWriter outputFile = new StreamWriter(path);
                await outputFile.WriteAsync(json);
            }
            catch (Exception ex)
            {
                logger.LogError("Serializer was not able to serialize object to file.", ex);
            }
        }

        /**
         * <summary>Deserializes json loaded from a file to a <see cref="Folder"/> object.</summary>
         * <param name="folder">Given folder structure.</param>
         * <param name="path">Path to which file should be saved.</param>
         */
        public async Task<Folder?> DeserializeFolderStructureFromFile(string path)
        {
            if (path == null)
            {
                logger.LogWarning("Serializer was not able to deserialiaze file to a Folder object. Path parameter is null.");
                return null;
            }

            try
            {
                using StreamReader inputFile = new StreamReader(path);
                return Deserialize(await inputFile.ReadToEndAsync());
            }
            catch (Exception ex)
            {
                logger.LogError("Serializer was not able to deserialize file to a Folder object.", ex);
                return null;
            }
        }
    }

}
