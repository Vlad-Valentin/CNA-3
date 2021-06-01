using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ChatClient.Views
{
    public partial class EntryWindow : Window
    {
        public EntryWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Close();
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Bear_MouseDown(object sender, MouseEventArgs e)
        {
            Random random = new();

            var names = new List<string> { "CoolBear", "BugBear", "VanillaBear",
            "Filip", "Tupi", "Max", "Valent", "Minmo", "Steve"};

            UsernameText.Text = names[random.Next(names.Count)];
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(UsernameText.Text);
            mainWindow.Show();

            Close();
        }
    }
}