using ConsoleApplication.model;
using ConsoleApplication.serialization;
using ConsoleApplication.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ConsoleApplication.util
{
    public class ConsoleUtil
    {

        public static void MainLoop()
        {
            LoadingService LoadingService = new LoadingService();
            CustomJsonSerializer jsonSerializer = new CustomJsonSerializer();

            while (true)
            {
                Console.WriteLine("Please provide a folder path or JSON file containing directory information: ");
                string input = Console.ReadLine();

                string json = jsonSerializer.Serialize(LoadingService.LoadUpFolderContent(input)); 
                Console.WriteLine(json);

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
            }
        }

    }
}
