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
            catch (NotSupportedException e)
            {
                // log
                return null;
            }
        }
    }

}
