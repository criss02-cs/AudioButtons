using AudioButtons.Animations;
using AudioButtons.Models;
using AudioButtons.ViewModels;
using System.Reflection;
using CA.Maui.Components;

namespace AudioButtons
{
    public partial class MainPage
    {
        private ButtonWidthAnimation _buttonWidthAnimation;
        public MainPage()
        {
            InitializeComponent();
            var viewModel = Application.Current.Handler.MauiContext.Services.GetService(typeof(ButtonsViewModel)) as ButtonsViewModel;
            BindingContext = viewModel;
        }

        private void Button_OnPressed(object sender, EventArgs e)
        {
            var button = (Button)sender;
            ViewModel.PlayButtonAsyncCommand.Execute(button.BindingContext as ButtonAudio);
            if (_buttonWidthAnimation is not null && !_buttonWidthAnimation.IsAnimationReset)
            {
                MediaElement.Stop();
                _buttonWidthAnimation.Reset(this, "WidthAnimation", 16U, 250U, Easing.Linear, (d, b) =>
                {
                    PlayButton(button);
                });
            }

            if (_buttonWidthAnimation is null)
            {
                PlayButton(button);
            }
            
        }

        private void PlayButton(Button button)
        {
            _buttonWidthAnimation =
                new ButtonWidthAnimation(v => button.WidthRequest = v, button.Width, CalculatePercentage(85));
            _buttonWidthAnimation.Start(this, "WidthAnimation", 16U, 250U, Easing.Linear);
            MediaElement.Play();
            ViewModel.IsPauseButtonVisible = true;
            ViewModel.IsStopButtonVisible = true;
            ViewModel.IsPlayButtonVisible = false;
        }
        
        private double CalculatePercentage(double percentage)
        {
            return percentage / 100 * buttonsCollection.Width;
        }

        private void MediaElement_OnMediaEnded(object sender, EventArgs e)
        {
            _buttonWidthAnimation.Reset(this, "WidthAnimation", 16U, 250U, Easing.Linear,
                (d, b) => _buttonWidthAnimation = null);
            ViewModel.IsPauseButtonVisible = false;
            ViewModel.IsStopButtonVisible = false;
            ViewModel.IsPlayButtonVisible = false;
        }

        private ButtonsViewModel ViewModel => BindingContext as ButtonsViewModel;

        private void MainPage_OnNavigatedTo(object sender, NavigatedToEventArgs e)
        {
            var page = PreviousPage(e);
            if (page != null)
            {
                ViewModel.LoadButtonsCommand.Execute(null);
            }
        }

        private Page PreviousPage(NavigatedToEventArgs e)
        {
            Type type = e.GetType();
            PropertyInfo internalProperty = type.GetProperty("PreviousPage", BindingFlags.NonPublic | BindingFlags.Instance);

            if (internalProperty != null)
            {
                Page previousPage = (Page)internalProperty.GetValue(e);
                return previousPage;
            }

            return null;
        }


        private void StopButton(object sender, TappedEventArgs e)
        {
            MediaElement.Stop();
            MediaElement_OnMediaEnded(null, null);
        }

        private void PauseButton(object sender, TappedEventArgs e)
        {
            MediaElement.Pause();
            ViewModel.IsPauseButtonVisible = false;
            ViewModel.IsStopButtonVisible = true;
            ViewModel.IsPlayButtonVisible = true;
        }

        private void PlayButton(object sender, TappedEventArgs tappedEventArgs)
        {
            MediaElement.Play();
            ViewModel.IsPauseButtonVisible = true;
            ViewModel.IsStopButtonVisible = true;
            ViewModel.IsPlayButtonVisible = false;
        }

        private void CaButton_OnLongPressed(object sender, EventArgs e)
        {
            if (sender is CaButton button)
            {
                ViewModel.ModifyButton.EnableExecution();
                ViewModel.ModifyButton.Execute(button.LongCommandParameter);
            }
        }
    }
}