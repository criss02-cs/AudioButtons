using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioButtons.AsyncEvents;
using AudioButtons.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace AudioButtons.ViewModels
{
    public partial class ButtonViewModel : ObservableObject
    {
        private readonly ButtonAudio _button;
        private Database _db;

        public string Name
        {
            get => _button.Name;
            set
            {
                if (_button.Name == value) return;
                _button.Name = value;
                OnPropertyChanged();
            }
        }

        public string AudioPath
        {
            get => _button.Audio.FilePath;
            set
            {
                if (_button.Audio.FilePath == value) return;
                _button.Audio.FilePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlayLoop
        {
            get => _button.Audio.PlayLoop;
            set
            {
                if (_button.Audio.PlayLoop == value) return;
                _button.Audio.PlayLoop = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get => _button.Color;
            set
            {
                if(_button.Color == value) return;
                _button.Color = value;
                OnPropertyChanged();
            }
        }

        public event EventHandlerAsync EndLoadEvent;

        public ICommand PlayAudio { get; private set; }
        public ICommand LoadAudio { get; private set; }
        
        public ButtonViewModel(Database db)
        {
            _db = db;
            _button = new ButtonAudio();
            LoadAudio = new AsyncRelayCommand(OpenAudioFromFile);
        }
        
        public ButtonViewModel(ButtonAudio button) => _button = button;

        private async Task OpenAudioFromFile()
        {
            try
            {
                var audioFileType =
                    new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { "*.mp3", "*.m4a" } },
                        { DevicePlatform.Android, new[] { "audio/*" } },
                        { DevicePlatform.iOS, new[] { "public.audio" } },
                        { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
                    });
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Seleziona un file audio",
                    FileTypes = audioFileType,
                });
                if (result is not null)
                {
                    await _db.Init();
                    _button.Audio.FilePath = result.FullPath;
                    _button.SerializedAudio = JsonConvert.SerializeObject(_button.Audio);
                    _button.Id = Guid.NewGuid();
                    var rows = await _db.SaveItemAsync(_button);
                    //EndLoadEvent?.Invoke(this, new CustomEventArgs<bool> { Value = rows > 0 });
                    await Back();
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Errore", e.Message, "Ok");
            }
        }

        [RelayCommand]
        Task Back() => Shell.Current.GoToAsync("..");
    }
}
