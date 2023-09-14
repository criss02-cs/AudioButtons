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
    [QueryProperty(nameof(Button), nameof(Button))]
    public partial class ButtonViewModel : ObservableObject
    {
        private Database _db;
        [ObservableProperty]
        ButtonAudio _button;

        public string FilePath
        {
            get => Button.FilePath;
            set
            {
                if (Button.FilePath == value) return;
                Button.FilePath = value;
                OnPropertyChanged();
                SaveFileCommand.NotifyCanExecuteChanged();
            }
        }

        public string Color
        {
            get => Button.Color;
            set
            {
                if (Button.Color == value) return;
                Button.Color = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => Button.Name;
            set
            {
                if (Button.Name == value) return;
                Button.Name = value;
                OnPropertyChanged();
                SaveFileCommand.NotifyCanExecuteChanged();
            }
        }
        public ButtonViewModel(Database db)
        {
            _db = db;
        }

        private bool CanSave() => _button is not null && !string.IsNullOrEmpty(Button.Name) && !string.IsNullOrEmpty(Button.FilePath);

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveFile()
        {
            await _db.Init();
            var rows = 0;
            if (Button.Id == Guid.Empty)
            {
                Button.Id = Guid.NewGuid();
                rows = await _db.SaveItemAsync(Button);
            }
            else
            {
                //rows = _db
            }
            if (rows == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Errore", "C'è stato un problema nel salvataggio del bottone", "Ok");
            }
            else
            {
                await Back();
            }
        }
        [RelayCommand]
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
                    FilePath = result.FullPath;
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Errore", e.Message, "Ok");
            }
        }

        [RelayCommand]
        private Task Back() => Shell.Current.GoToAsync("..");
    }
}
