using System;
using System.Windows;

namespace UseEntity
{
    /// <summary>
    /// Логика взаимодействия для AddArtist.xaml
    /// </summary>
    public partial class AddArtist
    {
        public AddArtist( )
        {
            InitializeComponent( );
            new Presenter(this);
        }

        public event EventHandler NewGroup;

        private void AddGroup_OnClick(object sender, RoutedEventArgs e)
        {
            NewGroup?.Invoke(sender, e);
        }
    }
}
