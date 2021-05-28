using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChatClient.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        protected void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> userList;
        public ObservableCollection<string> UserList
        {
            get => userList;

            set
            {
                userList = value;
                Notify(nameof(UserList));
            }
        }

        private ObservableCollection<string> messageList;

        public MainWindowVM()
        {
            UserList = new();
            MessageList = new();
        }

        public ObservableCollection<string> MessageList
        {
            get => messageList;

            set
            {
                messageList = value;
                Notify(nameof(MessageList));
            }
        }
    }
}