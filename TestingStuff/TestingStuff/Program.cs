using System;
using System.IO;

namespace TestingStuff
{
    class Program
    {
        public static void Main(string[] args)
        {
            var markov = new Markov.MarkovModel();

            while (true) {
                var command = Console.ReadLine().ToLower().Split(' ');

                if (command.Length > 1) {
                    switch (command[0]) {
                        case "train":
                            string path = command[1];

                            var lines = File.ReadAllLines(path);
                            foreach (var line in lines) {
                                markov.TrainOnData(line);
                            }
                            break;
                        case "generate":
                            int amount = Int32.Parse(command[1]);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "output.txt", markov.GenerateSequences(amount));
                            break;
                        case "save":
                            markov.Save(command[1]);
                            break;
                        case "load":
                            markov.Load(command[1]);
                            break;
                        default:
                            Console.WriteLine("Unknown command.");
                            continue; 
                    }
                    Console.WriteLine("Done");
                } else if (command.Length == 1) {
                    switch (command[0]) {
                        case "show":
                            Console.WriteLine(markov.GenerateSequences(1));
                            break;
                    }
                }
            }
        }
    }
}
