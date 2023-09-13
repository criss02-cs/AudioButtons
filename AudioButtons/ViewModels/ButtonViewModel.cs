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
    [QueryProperty("Button", "Button")]
    public partial class ButtonViewModel : BaseViewModel
    {
        private Database _db;

        [ObservableProperty] 
        [NotifyPropertyChangedFor(nameof(IsSaveButtonEnabled))]
        private ButtonAudio button = new ButtonAudio();
        public bool IsSaveButtonEnabled => !string.IsNullOrEmpty(Button.Name) && !string.IsNullOrEmpty(Button.Audio.FilePath);
        public ButtonViewModel(Database db)
        {
            _db = db;
            Button.Audio ??= new Audio();
        }
        [RelayCommand]
        private async Task SaveFile()
        {
            await _db.Init();
            Button.Id = Guid.NewGuid();
            var rows = await _db.SaveItemAsync(Button);
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
                    Button.Audio.FilePath = result.FullPath;
                    Button.SerializedAudio = JsonConvert.SerializeObject(Button.Audio);
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
