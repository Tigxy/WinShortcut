using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace win_short_cut.Pages
{
    internal class PageManager : INotifyPropertyChanged
    {
        // Helper for thread safety
        private static object _lock = new();

        private static PageManager? _instance;
        public static PageManager Instance
        {
            get { 
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new PageManager();
                    }
                }
                return _instance;
            }
            set { _instance = value; }
        }

        private ILoadablePage? _currentPage;
        public ILoadablePage? CurrentPage
        {
            get => _currentPage;
            set { 
                _currentPage = value; 
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private Dictionary<string, ILoadablePage> Pages { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private PageManager()
        {
            
        }

        public void RegisterPage(string name, ILoadablePage page)
        {
            if (Pages.ContainsKey(name))
                throw new ArgumentException($"Page '{name}' is already registered!");
            Pages.Add(name, page);
        }

        public void SwitchToPage(string name) => SwitchToPage(name, Array.Empty<object>());
        public void SwitchToPage(string name, params object[] parameters)
        {
            CurrentPage = GetPage(name);
            CurrentPage.LoadPage(parameters);
        }

        public ILoadablePage GetPage(string name)
        {
            if (!Pages.ContainsKey(name))
                throw new ArgumentException($"Page '{name}' is not registered!");

            return Pages[name];
        }
    }
}
