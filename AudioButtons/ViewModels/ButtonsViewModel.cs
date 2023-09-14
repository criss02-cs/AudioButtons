using System.Collections.ObjectModel;
using System.Diagnostics;
using AudioButtons.Models;
using AudioButtons.Views;
using CA.Maui.Commands;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace AudioButtons.ViewModels
{
    public partial class ButtonsViewModel : BaseViewModel
    {
        private Database _db;
        public ObservableCollection<ButtonAudio> Buttons { get; private set; } = new();
        public ICaCommand ModifyButton { get; }
        [ObservableProperty] private MediaSource mediaSource;
        [ObservableProperty] private bool isPlayButtonVisible = false;
        [ObservableProperty] private bool isPauseButtonVisible = false;
        [ObservableProperty] private bool isStopButtonVisible = false;

        public ButtonsViewModel(Database db)
        {
            _db = db;
            Title = "Bottoni sonori";
            Task.Run(LoadButtons);
            ModifyButton = new CaCommand<ButtonAudio>(GoToPage);
        }

        [RelayCommand]
        private void DeleteButtonAsync(ButtonAudio button)
        {
            if (!Buttons.Contains(button))
            {
                return;
            }

            Buttons.Remove(button);
            // TODO eliminare il bottone anche dal db
        }
        [RelayCommand]
        private Task AddNewButtonAsync() => Shell.Current.GoToAsync(nameof(ButtonPage), new Dictionary<string, object>
        {
            ["Button"] = new ButtonAudio()
        });
        [RelayCommand]
        private void PlayButtonAsync(ButtonAudio button)
        {
            if (button.FilePath.Length == 0)
            {
                return;
            }

            MediaSource = MediaSource.FromFile(button.FilePath);
        }
        [RelayCommand]
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

        private void GoToPage(ButtonAudio button)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                ["Button"] = button
            };
            Shell.Current.GoToAsync(nameof(ButtonPage), navigationParameter);
        }
    }
}