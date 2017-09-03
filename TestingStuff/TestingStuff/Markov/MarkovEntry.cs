using System;
using System.Collections.Generic;

namespace TestingStuff.Markov
{
    public class MarkovEntry
    {
        public int totalCount = 1;
        public List<Entry> Entries = new List<Entry>();
        static Random rng = new Random();

        public static readonly string SEPARATOR = ((char)9).ToString();

        public void Add(string value)
        {
            for (int i = 0; i < this.Entries.Count; i++) {
                if (this.Entries[i].Value == value) {
                    this.Entries[i].Count++;
                    this.totalCount++;
                    return;
                }
            }

            this.Entries.Add(new Entry(value));
        }

        public string GetNext()
        {
            int index = rng.Next() % Entries.Count;
            int chance = rng.Next() % totalCount;

            while (this.Entries[index].Count < chance) {
                index = (index + 1) % Entries.Count;
                chance = rng.Next() % totalCount;
            }

            return this.Entries[index].Value;
        }
    }
    
    public class Entry
    {
        public int Count = 1;
        public string Value;

        public Entry(string value)
        {
            this.Value = value;
        }
    }
}
