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
}
