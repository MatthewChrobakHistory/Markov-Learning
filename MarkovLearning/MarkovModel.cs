using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovLearning
{
    public class MarkovModel<T> where T : IConcatable
    {
        private Dictionary<T, MarkovModelEntry<T>> _entries = new Dictionary<T, MarkovModelEntry<T>>();

        public void Train(T[] keys)
        {

        }

        public void Save(string path)
        {

        }

        public void Load(string path)
        {

        }

        public T GenerateSequences(int numSequences)
        {
            T output = default(T);

            while (numSequences-- > 0) {
                output = (T)output.Concat(this.GenerateSequence());
            }

            return output;
        }




        private void Add(T key, T entry)
        {
            if (!this._entries.ContainsKey(key)) {
                this._entries[key] = new MarkovModelEntry<T>();
            }

            this._entries[key].Add(entry);
        }

        private T GetFollowing(T key)
        {

        }

        private T GenerateSequence()
        {

        }
    }
}
