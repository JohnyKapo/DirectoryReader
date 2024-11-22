using ConsoleApplication.model;
using ConsoleApplication.serialization;
using ConsoleApplication.service;
using ConsoleApplication.util.logger;

namespace ConsoleApplication.util
{
    /**
     * <summary>Class <b>ConsoleUtil</b> is used as a interactive console UI for main processes related to reading directory structure and de/serializing input/output.</summary>
     */
    public class ConsoleUtil
    {
        private static Logger logger = Logger.GetInstance();

        /**
         * <summary><b>MainLoop</b> function acts as a main logic of this program.</summary>
         */
        public void MainLoop()
        {
            LoadingService LoadingService = new LoadingService();
            CustomJsonSerializer jsonSerializer = new CustomJsonSerializer();
            logger.LogDebug("Starting application ...");

            while (true)
            {
                Console.WriteLine("Please provide a folder path or JSON file containing directory information: ");
                string input = Console.ReadLine();

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Folder folder = LoadingService.LoadUpFolderContent(input);
                if (folder == null)
                {
                    Console.WriteLine("Invalid path to a folder. Please try again.");
                    continue;
                }

                Console.WriteLine("Extensions found in folder: " + folder.Name);
                PrintPostfixes(folder.NestedPostfixes);
                input = SaveFolderStructure(jsonSerializer, folder);

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.WriteLine("------------------------------------------------------------------------------");
            }

            logger.LogDebug("Application reached exit state.");
        }

        /**
         * <summary>Function checks whether user wants to save a serialized JSON to a file.</summary>
         */
        private string? SaveFolderStructure(CustomJsonSerializer jsonSerializer, Folder folder)
        {
            Console.WriteLine("Would you like to save the folder to JSON? [y(es)/n(o)]");
            string saveInput = Console.ReadLine();

            bool termTrigger = false;
            while (!termTrigger)
            {
                switch (saveInput)
                {
                    case "y":
                    case "yes":
                        termTrigger = true;
                        SerializeJsonToFile(folder, jsonSerializer);
                        break;
                    case "n":
                    case "no":
                        termTrigger = true;
                        break;
                    default:
                        Console.WriteLine("Please try again. Invalid character, please pick from y(es)/n(o).");
                        termTrigger = false;
                        saveInput = Console.ReadLine();
                        break;
                }
            }
            return saveInput;
        }

        /**
         * <summary>Function serializes JSON to a file.</summary>
         */
        private void SerializeJsonToFile(Folder folder, CustomJsonSerializer jsonSerializer)
        {
            jsonSerializer.SerializeToFile(folder, "output.json");
        }

        /**
         * <summary>Function prints postfixes.</summary>
         */
        private void PrintPostfixes(List<Postfix> postfixes)
        {
            foreach (Postfix postfix in postfixes) {
                Console.WriteLine("Type: " + postfix.PostfixVal + " | " + "Count: " + postfix.Count);
                Console.WriteLine("--");
            }
        }

    }
}
