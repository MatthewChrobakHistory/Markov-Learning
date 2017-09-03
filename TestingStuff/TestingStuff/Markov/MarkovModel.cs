using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TestingStuff.Markov
{
    public class MarkovModel
    {
        Dictionary<string, MarkovEntry> Entries = new Dictionary<string, MarkovEntry>();

        public void Add(string key, string entry)
        {
            if (!this.Entries.ContainsKey(key)) {
                Entries[key] = new MarkovEntry();
            }

            Entries[key].Add(entry);
        }

        public string GetFollowing(string key)
        {
            if (this.Entries.ContainsKey(key)) {
                return Entries[key].GetNext();
            } else {
                System.Console.WriteLine("Couldn't find following for " + key);
                return string.Empty;
            }
        }

        public string GenerateSequences(int numSequences)
        {
            string output = string.Empty;

            for (int i = 0; i < numSequences; i++) {
                output += GenerateSequence();
            }

            return output;
        }

        private string GenerateSequence()
        {
            string output = string.Empty;
            string prev = string.Empty;
            string key = this.GetFollowing(MarkovEntry.SEPARATOR);

            while (key != MarkovEntry.SEPARATOR || output.Trim() == string.Empty) {
                if (key != prev) {
                    output += $"{key} ";
                    prev = key;
                }
                key = this.GetFollowing(key);
            }

            return output.Trim();
        }

        public void TrainOnData(string line)
        {
            Regex rx = new Regex("\\s+");

            var keys = rx.Split(line);


            for (int i = 0; i < keys.Length; i++) {
                string value;
                string following;

                if (i == 0) {
                    value = MarkovEntry.SEPARATOR;
                } else {
                    value = keys[i];
                }

                if (i == keys.Length - 1) {
                    following = MarkovEntry.SEPARATOR;
                } else {
                    following = keys[i + 1];
                }

                this.Add(value, following);
            }
        }


        public void Save(string path)
        {
            string output = string.Empty;

            foreach (var key in this.Entries.Keys) {
                output += $"{key} {this.Entries[key].totalCount}";

                foreach (var entry in this.Entries[key].Entries) {
                    output += $" {entry.Value} {entry.Count}";
                }

                output += "\n";
            }

            File.WriteAllText(path, output);
        }

        public void Load(string path)
        {
            string output = File.ReadAllText(path);

            var markovEntries = output.Split('\n');


            for (int x = 0; x < markovEntries.Length; x++) {
                if (markovEntries[x].Trim() != string.Empty) {
                    var keywords = markovEntries[x].Split(' ');
                    var newMarkovEntry = new MarkovEntry() { totalCount = int.Parse(keywords[1]) };

                    for (int i = 0; i < keywords.Length; i += 2) {
                        string value = keywords[i];
                        int count = int.Parse(keywords[i + 1]);

                        newMarkovEntry.Entries.Add(new Entry(value) { Count = count });
                    }

                    this.Entries.Add(keywords[0], newMarkovEntry);
                }
            }
        }
    }
}
