using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestingStuff.Markov;

namespace MarkovLearning
{
    public class EclipseChatTrainer
    {
        public static void Main(string[] args)
        {
            while (true) {
                string[] users = Console.ReadLine().Split(',');
                int numLines = int.Parse(Console.ReadLine());

                GenChatBetweenUsers(users, numLines);
            }
        }

        public static void GenChatBetweenUsers(string[] userNames, int numLines)
        {
            List<MarkovModel> users = new List<MarkovModel>(userNames.Length);
            string outputFile = string.Empty;

            for (int i = 0; i < userNames.Length; i++) {
                string username = userNames[i];
                string file = "users\\" + username + ".dat";

                users.Add(null);

                if (File.Exists(file)) {
                    outputFile += username + ",";
                    users[i] = new MarkovModel();
                    users[i].Load(file);
                }
            }

            Random rng = new Random();
            string output = string.Empty;

            for (int i = 0; i < numLines; i++) {
                int index = rng.Next() % users.Count;
                output += userNames[index] + ": " + users[index].GenerateSequences(1) + "\r\n";
            }

            File.WriteAllText(outputFile + numLines + ".txt", output);
        }

        public static void GenAllUsers()
        {
            string logsFolder = System.Console.ReadLine();
            Dictionary<string, MarkovModel> users = new Dictionary<string, MarkovModel>();

            foreach (string folder in Directory.GetDirectories(logsFolder)) {
                foreach (string file in Directory.GetFiles(folder)) {
                    if (!file.EndsWith("total.txt")) {

                        var fi = new FileInfo(file);

                        string username = fi.Name.Remove(fi.Name.IndexOf('.'));

                        if (!users.ContainsKey(username)) {
                            Console.WriteLine("Adding " + username);
                            users.Add(username, new MarkovModel());
                        }

                        string[] lines = File.ReadAllLines(file);

                        foreach (var line in lines) {
                            string markovLine = line.Substring(line.IndexOf(':') + 1);
                            users[username].TrainOnData(markovLine);
                        }
                    }
                }
            }

            foreach (string key in users.Keys) {
                users[key].Save(key + ".dat");
            }
        }

        public static void GenEclipse()
        {
            var markov = new TestingStuff.Markov.MarkovModel();

            string logsFolder = System.Console.ReadLine();

            foreach (string folder in Directory.GetDirectories(logsFolder)) {
                string logFile = folder + "/total.txt";

                string[] lines = File.ReadAllLines(logFile);

                System.Console.WriteLine("Doing " + folder);

                foreach (var line in lines) {
                    string markovLine = line.Substring(line.IndexOf(':') + 1);
                    markov.TrainOnData(markovLine);
                }
            }


            markov.Save("eclipse.dat");
        }
    }
}
