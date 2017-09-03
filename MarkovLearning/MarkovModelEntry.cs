using System;
using System.Collections.Generic;

namespace MarkovLearning
{
    public class MarkovModelEntry<T> where T : IConcatable
    {
        public int TotalCount = 1;
        public List<Entry<T>> Entries = new List<Entry<T>>();

        private static Random RNG = new Random();

        public void Add(T value)
        {
            for (int i = 0; i < this.Entries.Count; i++) {
                if (this.Entries[i].Value.Equals(value)) {
                    this.Entries[i].Count++;
                    this.TotalCount++;
                    return;
                }
            }

            this.Entries.Add(new Entry<T>(value));
        }

        public T GetNext()
        {
            int index = RNG.Next() % this.Entries.Count;
            int chance = RNG.Next() % this.TotalCount;

            while (this.Entries[index].Count < chance) {
                index = (index + 1) % this.Entries.Count;
                chance = RNG.Next() % TotalCount;
            }

            return this.Entries[index].Value;
        }
    }

    public class Entry<T>
    {
        public int Count = 1;
        public T Value;

        public Entry(T value)
        {
            this.Value = value;
        }
    }
}
