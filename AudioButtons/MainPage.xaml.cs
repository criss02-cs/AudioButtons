using AudioButtons.Animations;
using AudioButtons.Components;
using AudioButtons.Models;
using AudioButtons.ViewModels;
using System.Reflection;

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
            var viewModel = BindingContext as ButtonsViewModel;
            viewModel.PlayButton.Execute(button.BindingContext as ButtonAudio);
            _buttonWidthAnimation =
                new ButtonWidthAnimation(v => button.WidthRequest = v, button.Width, CalculatePercentage(90));
            _buttonWidthAnimation.Start(this, "WidthAnimation", 16U, 250U, Easing.Linear);
        }

        private double CalculatePercentage(double percentage)
        {
            return percentage / 100 * buttonsCollection.Width;
        }

        private void MediaElement_OnMediaEnded(object sender, EventArgs e)
        {
            _buttonWidthAnimation.Reset(this, "WidthAnimation", 16U, 250U, Easing.Linear);
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
    }
}