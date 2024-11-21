using ConsoleApplication.model;
using ConsoleApplication.serialization;
using ConsoleApplication.service;

namespace ConsoleApplication.util
{
    public class ConsoleUtil
    {

        public void MainLoop()
        {
            LoadingService LoadingService = new LoadingService();
            CustomJsonSerializer jsonSerializer = new CustomJsonSerializer();

            while (true)
            {
                Console.WriteLine("Please provide a folder path or JSON file containing directory information: ");
                string input = Console.ReadLine();

                Folder folder = LoadingService.LoadUpFolderContent(input);
                PrintPostfixes(folder.NestedPostfixes);

                Console.WriteLine("Extensions found in folder: " + input);
                Console.WriteLine("Would you like to save the folder to JSON? [y(es)/n(o)]");
                input = Console.ReadLine();

                bool termTrigger = false;
                while (!termTrigger)
                {
                    switch (input)
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
                            input = Console.ReadLine();
                            break;
                    }
                }

                Console.WriteLine("------------------------------------------------------------------------------");
            }
        }

        private void SerializeJsonToFile(Folder folder, CustomJsonSerializer jsonSerializer)
        {
            Console.WriteLine("Please specify path, where the the file should be saved: ");
            string pathInput = Console.ReadLine();
            jsonSerializer.SerializeToFile(folder, pathInput);
        }

        private void PrintPostfixes(List<Postfix> postfixes)
        {
            foreach (Postfix postfix in postfixes) {
                Console.WriteLine("Type: " + postfix.PostfixVal + " | " + "Count: " + postfix.Count);
                Console.WriteLine("--");
            }
        }

    }
}
