using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_short_cut.Utils
{
    public static class Collections
    {
        public static void ForEach<T>(this T[] array, Action<T> action) => Array.ForEach(array, action);

        public static void ForEach<T>(this ObservableCollection<T> collection, Action<T> action) {
            foreach (var item in collection) {
                action(item);
            }
        }
    }
}
