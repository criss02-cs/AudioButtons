using System.Diagnostics;
using CA.Maui.Commands;


namespace CA.Maui.Components
{
    public class CaButton : Button
    {
        private readonly Stopwatch _timer = new();

        public static readonly BindableProperty ThresholdProperty = BindableProperty.Create(
            nameof(Threshold), typeof(int), typeof(CaButton), 500);
        public static readonly BindableProperty CaCommandProperty = BindableProperty.Create(
            nameof(CaCommand), typeof(ICaCommand), typeof(CaButton), null);
        public ICaCommand CaCommand { get => (ICaCommand)GetValue(CaCommandProperty); set => SetValue(CaCommandProperty, value); }
        public int Threshold { get => (int)GetValue(ThresholdProperty); set => SetValue(ThresholdProperty, value); }

        public CaButton()
        {
            Pressed += OnButtonPressed;
            Released += OnButtonReleased;
            Clicked += OnClicked;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            CaCommand?.Execute(CommandParameter);
        }

        private void OnButtonPressed(object sender, EventArgs e)
        {
            _timer.Start();
        }

        private void OnButtonReleased(object sender, EventArgs e)
        {
            if (_timer.ElapsedMilliseconds >= Threshold)
            {
                CaCommand?.DisableExecution();
                LongPressed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CaCommand?.EnableExecution();
                ShortPressed?.Invoke(this, EventArgs.Empty);
            }
            _timer.Stop();
            _timer.Reset();
        }

        public event EventHandler LongPressed;
        public event EventHandler ShortPressed;
    }
}
