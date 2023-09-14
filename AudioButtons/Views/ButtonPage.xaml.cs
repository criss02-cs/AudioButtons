using AudioButtons.ViewModels;

namespace AudioButtons.Views;

public partial class ButtonPage : ContentPage
{
	public ButtonPage(ButtonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        Console.WriteLine(NameEntry.Text.Length);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private ButtonViewModel ViewModel => BindingContext as ButtonViewModel;

}