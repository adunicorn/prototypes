using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Client.Models;
using RestSharp;

namespace Client.ViewModels
{
    public enum TileStatus
    {
        Loading = 0,
        Success = 1,
        Failed = -1
    }
    public class Tile : INotifyPropertyChanged, IDisposable
    {
        public TileStatus _status;
        public Card _card;

        public Card Card
        {
            get { return _card; }
            set
            {
                _card = value;
                OnPropertyChanged();
            }
        }
        public TileStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged("IsLoading");
                OnPropertyChanged("IsSuccess");
                OnPropertyChanged("IsFailed");
            }
        }

        public bool IsLoading
        {
            get
            {
                return _status == TileStatus.Loading;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return _status == TileStatus.Success;
            }
        }

        public bool IsFailed
        {
            get
            {
                return _status == TileStatus.Failed;
            }
        }

        private MainWindowViewModel _viewModel;
        public Tile(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;

            Load();
        }

        private void DelayLoad()
        {
            Thread.Sleep(_viewModel.Settings.Delay);
            Load();
        }


        private BackgroundWorker _bw;
        private void Load()
        {
            _bw = new BackgroundWorker();

            _bw.DoWork += (evt, args) =>
            {
                Status = TileStatus.Loading;

                var client = new RestClient("http://localhost:58177");

                var request = new RestRequest("api/card", Method.GET);

                var response = client.Execute<Card>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Status = TileStatus.Success;
                    Card = response.Data;
                }
                else
                {
                    Status = TileStatus.Failed;
                }

                DelayLoad();
            };

            _bw.RunWorkerAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _bw.CancelAsync();
            _bw.Dispose();
        }
    }

    public class Settings : INotifyPropertyChanged
    {
        public int _tiles = 10;
        public int _delay = 5000;

        public int Tiles
        {
            get { return _tiles; }
            set
            {
                _tiles = value;
                OnPropertyChanged();
                _viewModel.Update(Tiles);
            }
        }

        public int Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
            }
        }

        private MainWindowViewModel _viewModel;
        public Settings(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Update(Tiles);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MainWindowViewModel
    {
        public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();
        public Settings Settings { get; set; }

        public MainWindowViewModel()
        {
             Settings = new Settings(this);                
        }

        public void Update(int count)
        {
            Tiles.Clear();

            for (int i = 0; i < count; i++)
            {
                Tiles.Add(new Tile(this));
            }
        }
    }
}
