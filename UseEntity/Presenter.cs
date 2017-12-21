using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace UseEntity
{
    class Presenter
    {
        private readonly MusicBase _musicBase = new MusicBase();
        private List<Artist> _artist;
        private List<Album> _album;
        private List<Style> _style;
        private static MainWindow _mainWindow;
        private readonly AddArtist _addArtist;
        private readonly AddAlbums _addAlbums;
        private readonly AddStyle _addStyle;
        private static readonly IMessageService _messageService = new MessageService( );

        private static string _name;

        #region Конструкторы
        public Presenter(MainWindow window)
        {
            _mainWindow = window;
            _mainWindow.MainWindowEvent += MainWindow_mainWindowEvent;
            _mainWindow.AddEvent += _mainWindow_AddEvent;
        }

        public Presenter(AddArtist addArtist)
        {
            _addArtist = addArtist;
            _artist = _musicBase.Artists.ToList( );
            _addArtist.NewGroup += _addArtist_NewGroup;
        }

        public Presenter(AddAlbums addAlbums)
        {
            _addAlbums = addAlbums;
            _artist = _musicBase.Artists.ToList( );
            _album = _musicBase.Albums.ToList( );
            _addAlbums.AddNewAlbum += _addAlbums_AddNewAlbum;
        }

        public Presenter(AddStyle addStyle)
        {
            _addStyle = addStyle;
            _artist = _musicBase.Artists.ToList( );
            _style = _musicBase.Styles.ToList( );
            _addStyle.AddNewStyle += _addStyle_AddNewStyle;
        }
        #endregion

        #region Добавление в БД
        private void _addStyle_AddNewStyle(object sender, EventArgs e)//Добавление стилей исполнителя
        {
            if (_addStyle.StyleBox.Text != "")
            {
                Artist instance = new Artist();
                foreach (var art in _artist)
                {
                    if (art.Name.ToUpper() == _name.ToUpper())
                        instance = art;
                }
                string[] temp = Regex.Split(_addStyle.StyleBox.Text, @"\b[!,#,$,%,',(,),*,+,\.,/,:,;,<,=,>,?,@,[,\\,\],^,_,{,},|]+\s*|\b\s{2,}");
                for (int i = 0; i < temp.Length; i++)
                {
                    bool flag = false;
                    foreach (var style in _style)
                    {
                        if (style.Name.ToUpper() == temp[i].ToUpper())
                        {
                            style.Artists.Add(instance);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        Style style = new Style();
                        style.Name = temp[i];
                        style.Artists.Add(instance);
                        _musicBase.Styles.Add(style);
                    }
                }
                _musicBase.SaveChanges();
                _addStyle.Close( );
            }
            else _messageService.ShowMessage("Вы не ввели ни одного стиля");
        }

        private void _addAlbums_AddNewAlbum(object sender, EventArgs e)//Добавление новых альбомов
        {
            if (_addAlbums.AlbumBox.Text != "" && _addAlbums.AlbumDateBox.Text != "")
            {
                try
                {
                    Album album = new Album();
                    album.Name = _addAlbums.AlbumBox.Text;
                    album.DateRelease = GetData(_addAlbums.AlbumDateBox.Text);
                    foreach (var art in _artist)
                    {
                        if (art.Name.ToUpper() == _name.ToUpper())
                        {
                            album.ArtistId = art.ArtistId;
                            album.Artist = art;
                        }
                    }
                    _musicBase.Albums.Add(album);
                    _musicBase.SaveChanges( );
                    MessageBoxResult result = _messageService.ShowExclametion("Вы ввели все альбомы?");
                    _addAlbums.Close( );
                    if (result == MessageBoxResult.Yes)
                    {
                        Window style = new AddStyle( );
                        style.Show( );
                    }
                    else
                    {
                        Window albums = new AddAlbums( );
                        albums.Show( );
                    }
                }
                catch (Exception exception)
                {
                    _messageService.ShowError(exception.Message);
                }
            }
            else _messageService.ShowMessage("Вы не ввели обязательные данные");
        }

        private static DateTime GetData(string str)
        {
            DateTime data;
            if(!DateTime.TryParse(str, out data))
                throw new ArgumentException();
            return data;
        }

        private void _addArtist_NewGroup(object sender, EventArgs e)//Добавление нового артиста
        {
            if (_addArtist.ArtistBox.Text != "" && _addArtist.AppeareanceBox.Text != "")
            {
                try
                {
                    bool flag = true;
                    foreach (var art in _artist)//TODO тут надо перебрать циклы. Слишком их много в одном месте
                    {
                        if (art.Name.ToUpper() == _addArtist.ArtistBox.Text.ToUpper())
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        Artist instance = new Artist();
                        instance.Name = _addArtist.ArtistBox.Text;
                        DateTime tempAppereance = GetData(_addArtist.AppeareanceBox.Text);
                        instance.Appearance = tempAppereance;
                        if (_addArtist.BreackUpBox.Text != "")
                            if (tempAppereance < GetData(_addArtist.BreackUpBox.Text))
                                instance.BreackUp = GetData(_addArtist.BreackUpBox.Text);
                            else _messageService.ShowMessage("Вы ввели некорректную вторую дату");

                        _musicBase.Artists.Add(instance);
                        _musicBase.SaveChanges( );
                        _messageService.ShowMessage("Исолнитель успешно добвален в базу");

                        //foreach (var artist in _artist)
                        //{
                        //    if (artist.Name.ToUpper() == _addArtist.ArtistBox.Text.ToUpper( ))
                        //    {
                        //        id = artist.ArtistId;
                        //    }
                        //}
                        _name = _addArtist.ArtistBox.Text;
                        _addArtist.Close( );
                        Window albums = new AddAlbums( );
                        albums.Show( );
                    }
                    else
                    {
                        _messageService.ShowMessage("Такой исполнитель уже есть в базе");
                        _addArtist.ArtistBox.Clear( );
                        _addArtist.AppeareanceBox.Clear( );
                        _addArtist.BreackUpBox.Clear( );
                    }
                }
                catch (Exception exception)
                {
                    _messageService.ShowError(exception.Message);
                }
            }
            else _messageService.ShowMessage("Вы не ввели обязательные данные");
        }
        #endregion

        #region MainWindow
        private void MainWindow_mainWindowEvent(object sender, EventArgs e)
        {

            try
            {
                if (_mainWindow.SearchBox.Text != "")
                {
                    _mainWindow.ArtistText.Text = "";
                    _mainWindow.AlbumText.Text = "";
                    _mainWindow.StyleText.Text = "";
                    //TODO Надо подумать как получше сделать
                    #region Поиск по названию                    

                        if (_mainWindow.ChoiseOfSearch.SelectedIndex == 00)
                        {
                            var result = _musicBase.Artists.FirstOrDefault(name =>
                                name.Name.ToUpper() == _mainWindow.SearchBox.Text.ToUpper());
                            if (result == null)
                                EmptyMessage();
                            else
                            {
                                _mainWindow.ArtistText.Text = result.ToString();
                                ShowAlbums(result.Albums);
                                ShowStyle(result.Styles);
                            }
                        }

                        #endregion

                        #region Поиск по стилю

                        else if (_mainWindow.ChoiseOfSearch.SelectedIndex == 1)
                        {
                            var result = _musicBase.Styles.FirstOrDefault(name =>
                                name.Name.ToUpper() == _mainWindow.SearchBox.Text.ToUpper());
                            if (result == null)
                                EmptyMessage( );
                            else
                            {
                                _mainWindow.StyleText.Text = result.ToString();
                                ShowArtist(result.Artists);
                            }
                        }

                        #endregion

                        #region Поиск по Альбому

                        else if (_mainWindow.ChoiseOfSearch.SelectedIndex == 2)
                        {
                            var result = _musicBase.Albums.FirstOrDefault(name =>
                                name.Name.ToUpper() == _mainWindow.SearchBox.Text.ToUpper());
                            if (result == null)
                                EmptyMessage( );
                            else
                            {
                                _mainWindow.AlbumText.Text = result.ToString();
                                _mainWindow.ArtistText.Text = result.Artist.ToString();
                                ShowStyle(result.Artist.Styles);
                            }
                        }

                        #endregion                       
                   
                }
                else _messageService.ShowMessage("Вы ничего не ввели");

            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }

        private static void ShowArtist(ICollection<Artist> artists)
        {
            foreach (var artist in artists)
            {
                _mainWindow.ArtistText.Text += artist + Environment.NewLine;
            }
        }

        private static void ShowStyle(ICollection<Style> styles)
        {
            foreach (var style in styles)
            {
                _mainWindow.StyleText.Text += style + Environment.NewLine;
            }
        }

        private static void ShowAlbums(ICollection<Album> albums)
        {
            foreach (var album in albums)
            {
                _mainWindow.AlbumText.Text += album + Environment.NewLine;
            }
        }

        private static void EmptyMessage()
        {
            _messageService.ShowMessage("Мы не нашли, то что вы ищите в базе");
        }

        private void _mainWindow_AddEvent(object sender, EventArgs e)
        {
            Window artist = new AddArtist( );
            artist.Show( );
        }
        #endregion
    }
}
