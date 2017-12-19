using System;
using System.Windows;

namespace UseEntity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow( )
        {
            InitializeComponent( );
            new Presenter(this);
        }

        public event EventHandler MainWindowEvent;
        public event EventHandler AddEvent;

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindowEvent?.Invoke(sender, e);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddEvent?.Invoke(sender, e);
        }
    }
}
