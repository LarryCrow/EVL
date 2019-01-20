using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Utils
{
    static class ObservableCollectionExtensions
    {
        public static ReadOnlyObservableCollection<T> AsReadOnly<T>(this ObservableCollection<T> collection) 
            => new ReadOnlyObservableCollection<T>(collection);
    }

    static class DictionaryExtensions
    {
        public static ReadOnlyDictionary<K,V> AsReadOnly<K,V>(this IDictionary<K,V> dictionary)
            => new ReadOnlyDictionary<K, V>(dictionary);
    }
}
