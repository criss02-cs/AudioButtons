using AudioButtons.ViewModels;

namespace AudioButtons.Views;

public partial class ButtonPage : ContentPage
{
	public ButtonPage()
	{
		InitializeComponent();
        var viewModel = Application.Current.Handler.MauiContext.Services.GetService(typeof(ButtonViewModel)) as ButtonViewModel;
		BindingContext = viewModel;
    }

	private ButtonViewModel ViewModel => BindingContext as ButtonViewModel;
}