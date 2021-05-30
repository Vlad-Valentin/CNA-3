using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChatClient.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public static ObservableCollection<string> UserList { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> MessageList { get; set; } = new ObservableCollection<string>();


        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}