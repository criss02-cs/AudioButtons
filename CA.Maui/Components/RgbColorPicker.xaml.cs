namespace CA.Maui.Components;

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
            if (!string.IsNullOrEmpty(color))
            {
                var colors = color.Replace("(", "").Replace(")", "").Split(',');
                control._red = Convert.ToByte(colors[0]);
                control._green = Convert.ToByte(colors[1]);
                control._blue = Convert.ToByte(colors[2]);
                control.RedSlider.Value = control._red;
                control.GreenSlider.Value = control._green;
                control.BlueSlider.Value = control._blue;
                control.Rgb = $"({control._red}, {control._green}, {control._blue})";
            }
            else
            {
                control._red = Convert.ToByte(0);
                control._green = Convert.ToByte(0);
                control._blue = Convert.ToByte(0);
                control.Rgb = $"({control._red}, {control._green}, {control._blue})";
            }
            //var control = (RgbColorPicker)bindable;
        }, defaultBindingMode: BindingMode.TwoWay);
    public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor), typeof(Color), typeof(RgbColorPicker));

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

    // metto la parola new per nascondere quello di base
    public new Color BackgroundColor { get => (Color)GetValue(BackgroundColorProperty); set => SetValue(BackgroundColorProperty, value); }

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