using AudioButtons.AsyncEvents;
using AudioButtons.ViewModels;

namespace AudioButtons.Components;

public partial class ButtonAudioComponent : ContentView
{
	public ButtonAudioComponent()
	{
		InitializeComponent();
        var viewModel = Application.Current.Handler.MauiContext.Services.GetService(typeof(ButtonViewModel)) as ButtonViewModel;
        BindingContext = viewModel;
        viewModel.EndLoadEvent += OnEndLoad;
    }

    public event EventHandlerAsync EndLoadEvent;
    private Task OnEndLoad(object sender, CustomEventArgs<bool> e)
    {
        return EndLoadEvent?.Invoke(this, e);
    }
}