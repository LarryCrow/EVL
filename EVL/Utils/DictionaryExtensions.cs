using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Utils
{
    static class DictionaryExtensions
    {
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }

    static class ObservableCollectionExtensions
    {
        public static ReadOnlyObservableCollection<T> AsReadOnly<T>(this ObservableCollection<T> collection) 
            => new ReadOnlyObservableCollection<T>(collection);
    }
}
