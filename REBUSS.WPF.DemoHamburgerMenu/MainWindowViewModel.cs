using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace REBUSS.WPF.DemoHamburgerMenu
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            MenuItems = new List<string>
            {
                Consts.List,
                Consts.Edit
            };
        }

        public IList<string> MenuItems { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
