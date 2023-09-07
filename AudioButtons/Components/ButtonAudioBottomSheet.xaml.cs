using AudioButtons.AsyncEvents;
using The49.Maui.BottomSheet;	

namespace AudioButtons.Components;

public partial class ButtonAudioBottomSheet : BottomSheet
{
	public ButtonAudioBottomSheet()
	{
		InitializeComponent();
	}

    private async Task ButtonAudioComponent_EndLoadEvent(object sender, CustomEventArgs<bool> e)
    {
		await DismissAsync();
    }
}