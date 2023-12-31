﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using AudioButtons.Models;
using AudioButtons.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace AudioButtons.ViewModels
{
    public class ButtonsViewModel : BaseViewModel
    {
        private Database _db;
        public ObservableCollection<ButtonAudio> Buttons { get; private set; } = new();
        
        public ICommand NewButton { get; }
        public ICommand DeleteButton { get; }
        public ICommand ModifyButton { get; }
        public ICommand PlayButton { get; }
        public ICommand LoadButtonsCommand { get; }

        private MediaSource _mediaSource;

        private bool _isPlayButtonVisible = false;
        private bool _isPauseButtonVisible = false;
        private bool _isStopButtonVisible = false;

        public MediaSource MediaSource
        {
            get => _mediaSource;
            set
            {
                if(value == _mediaSource) return;
                _mediaSource = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlayButtonVisible
        {
            get => _isPlayButtonVisible;
            set
            {
                if(value == _isPlayButtonVisible) return;
                _isPlayButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsPauseButtonVisible
        {
            get => _isPauseButtonVisible;
            set
            {
                if (value == _isPauseButtonVisible) return;
                _isPauseButtonVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsStopButtonVisible
        {
            get => _isStopButtonVisible;
            set
            {
                if (value == _isStopButtonVisible) return;
                _isStopButtonVisible = value;
                OnPropertyChanged();
            }
        }

        public ButtonsViewModel(Database db)
        {
            _db = db;
            Title = "Bottoni sonori";
            Task.Run(LoadButtons);
            DeleteButton = new AsyncRelayCommand<ButtonAudio>(DeleteButtonAsync);
            NewButton = new AsyncRelayCommand(AddNewButtonAsync);
            PlayButton = new AsyncRelayCommand<ButtonAudio>(PlayButtonAsync);
            LoadButtonsCommand = new AsyncRelayCommand(LoadButtons);
        }


        private async Task DeleteButtonAsync(ButtonAudio button)
        {
            if(!Buttons.Contains(button)) { return; }
            Buttons.Remove(button);
            // TODO eliminare il bottone anche dal db
        }

        private Task AddNewButtonAsync() => Shell.Current.GoToAsync(nameof(ButtonPage));

        private async Task PlayButtonAsync(ButtonAudio button)
        {
            if (button.Audio.FilePath.Length == 0)
            {
                return;
            }
            MediaSource = MediaSource.FromFile(button.Audio.FilePath);
        }

        private async Task LoadButtons()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                var list = await _db.GetAllButtons();
                list.ForEach(x => x.Audio = JsonConvert.DeserializeObject<Audio>(x.SerializedAudio));
                if (Buttons.Count > 0)
                {
                    Buttons.Clear();
                }
                list.ForEach(x => Buttons.Add(x));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unable to get buttons: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Errore!", e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
