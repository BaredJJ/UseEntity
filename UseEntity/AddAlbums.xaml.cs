using System;
using System.Windows;

namespace UseEntity
{
    /// <summary>
    /// Логика взаимодействия для AddAlbums.xaml
    /// </summary>
    public partial class AddAlbums
    {
        public AddAlbums( )
        {
            InitializeComponent( );
            new Presenter(this);
        }

        public event EventHandler AddNewAlbum;

        private void AddAlbum_OnClick(object sender, RoutedEventArgs e)
        {
            AddNewAlbum?.Invoke(sender, e);
        }
    }
}
