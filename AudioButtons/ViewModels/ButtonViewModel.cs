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

        //public bool IsNewButton => Button.Id != Guid.Empty;

        public ButtonViewModel(Database db)
        {
            _db = db;
            Button = new ButtonAudio();
        }

        [RelayCommand]
        private async Task DeleteButton()
        {
            await _db.Init();
            var rows = await _db.DeleteItemAsync(Button);
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
        private async Task SaveFile()
        {
            await _db.Init();
            var rows = 0;
            if (Button.Id == Guid.Empty)
            {
                Button.Id = Guid.NewGuid();
                rows = await _db.InsertItemAsync(Button);
            }
            else
            {
                rows = await _db.UpdateItemAsync(Button);
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
                    Button.FilePath = result.FullPath;
                    OnPropertyChanged(nameof(Button));
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
