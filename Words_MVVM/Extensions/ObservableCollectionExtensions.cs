using System.Linq;
using System.Collections.ObjectModel;

namespace Words_MVVM.Extensions
{
    public static class ObservableCollectionExtensions
    {
        // Sort an ObservableCollection in place.
        public static void Sort(this ObservableCollection<string> collection)
        {
            var tempSortedCollection = new ObservableCollection<string>(collection.OrderBy(text => text));
            collection.Clear();
            tempSortedCollection.ToList().ForEach(text => collection.Add(text));
        }
    }
}
