using System.Diagnostics;
using CA.Maui.Commands;


namespace CA.Maui.Components
{
    public class CaButton : Button
    {
        private readonly Stopwatch _timer = new();

        private static readonly BindableProperty ThresholdProperty = BindableProperty.Create(
            nameof(Threshold), typeof(int), typeof(CaButton), 500);

        private static readonly BindableProperty CaCommandProperty = BindableProperty.Create(
            nameof(CaCommand), typeof(ICaCommand), typeof(CaButton), null);

        private static readonly BindableProperty LongPressCommandProperty = BindableProperty.Create(
            nameof(LongPressCommand), typeof(ICaCommand), typeof(CaButton), null);

        private static readonly BindableProperty LongCommandParameterProperty = BindableProperty.Create(
            nameof(LongCommandParameter), typeof(object), typeof(CaButton), null);

        public ICaCommand CaCommand
        {
            get => (ICaCommand)GetValue(CaCommandProperty);
            set => SetValue(CaCommandProperty, value);
        }
        
        public ICaCommand LongPressCommand
        {
            get => (ICaCommand)GetValue(LongPressCommandProperty);
            set => SetValue(LongPressCommandProperty, value);
        }

        public object LongCommandParameter
        {
            get => GetValue(LongCommandParameterProperty);
            set => SetValue(LongCommandParameterProperty, value);
        }

        public int Threshold
        {
            get => (int)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

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
                LongPressCommand?.EnableExecution();
                LongPressCommand?.Execute(LongCommandParameter);
            }
            else
            {
                LongPressCommand?.DisableExecution();
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