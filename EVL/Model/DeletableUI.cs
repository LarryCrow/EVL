using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EVL.Model
{
    internal static class DeletableQueryExtensions
    {
        internal static IEnumerable<T> ExcludeDeleted<T>(this IEnumerable<DeletableUI<T>> query)
        {
            return query?.Where(e => !e.Deleted).Select(e => e.Value);
        }
    }

    public class DeletableUI<T> : INotifyPropertyChanged
    {
        private bool forceDeleted;
        private bool cascadeDeleted;

        public T Value { get; set; }

        public bool CascadeDeleted
        {
            get => cascadeDeleted;
            set
            {
                cascadeDeleted = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CascadeDeleted)));
            }
        }

        public bool ForceDeleted
        {
            get => forceDeleted;
            set
            {
                forceDeleted = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ForceDeleted)));
            }
        }

        public bool Deleted => ForceDeleted || CascadeDeleted;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
