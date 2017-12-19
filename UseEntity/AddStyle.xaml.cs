using System;
using System.Windows;

namespace UseEntity
{
    /// <summary>
    /// Логика взаимодействия для AddStyle.xaml
    /// </summary>
    public partial class AddStyle
    {
        public AddStyle( )
        {
            InitializeComponent( );
            new Presenter(this);
        }

        public event EventHandler AddNewStyle;

        private void StyleButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddNewStyle?.Invoke(sender, e);
        }
    }
}
