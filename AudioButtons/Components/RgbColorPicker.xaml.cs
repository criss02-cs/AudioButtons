namespace AudioButtons.Components;

public partial class RgbColorPicker : ContentView
{
    private byte _red = 0;
    private byte _green = 0;
    private byte _blue = 0;

    public static readonly BindableProperty RgbProperty = BindableProperty
        .Create(nameof(Rgb), typeof(string), typeof(RgbColorPicker), propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not RgbColorPicker control) return;
            var color = newValue as string;
            var colors = color.Replace("(", "").Replace(")", "").Split(',');
            control._red = Convert.ToByte(colors[0]);
            control._green = Convert.ToByte(colors[1]);
            control._blue = Convert.ToByte(colors[2]);
            control.Rgb = $"({control._red}, {control._green}, {control._blue})";
            //var control = (RgbColorPicker)bindable;
        });

    public RgbColorPicker()
    {
        InitializeComponent();
    }
    /// <summary>
    /// A string with the format (RED, GREEN, BLUE)
    /// </summary>
    public string Rgb
    {
        get => (string)GetValue(RgbProperty);
        set => SetValue(RgbProperty, value);
    }

    private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        var slider = sender as Slider;
        var newValue = Convert.ToByte(e.NewValue);
        if (slider == RedSlider) _red = newValue;
        else if (slider == GreenSlider) _green = newValue;
        else if (slider == BlueSlider) _blue = newValue;
        UpdateRgb();
    }
    
    private void UpdateRgb()
    {
        Rgb = $"({_red}, {_green}, {_blue}";
        Rectangle.Fill = new SolidColorBrush(Color.FromRgb(_red, _green, _blue));
    }
}