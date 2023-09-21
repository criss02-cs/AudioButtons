using System.Diagnostics;
using NAudio.Wave;

namespace CA.Maui.Components;

public class WaveformGraphicsView : GraphicsView
{
    public string Path { get => (string)GetValue(PathProperty); set => SetValue(PathProperty, value); }

    public Color LineColor { get => (Color)GetValue(LineColorProperty); set => SetValue(LineColorProperty, value); }

    public static readonly BindableProperty PathProperty = BindableProperty.Create(
        nameof(Path), typeof(string), typeof(WaveformGraphicsView), propertyChanged: PathPropertyChanged);

    public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
        nameof(LineColor), typeof(Color), typeof(WaveformGraphicsView), propertyChanged: LineColorPropertyChanged);

    private static void LineColorPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
    {
        if (bindable is not WaveformGraphicsView { Drawable: WaveformDrawable drawable } view)
        {
            return;
        }
        drawable.LineColor = newValue as Color;
        view.Invalidate();
    }

    public static void PathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not WaveformGraphicsView { Drawable: WaveformDrawable drawable } view)
        {
            return;
        }

        drawable.Path = newValue as string;
        view.Invalidate();
    }
}

public class WaveformDrawable : BindableObject, IDrawable
{
    public string Path { get; set; }
    public Color LineColor { get; set; }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        for (var i = 0; i < dirtyRect.Width; i += 10)
        {
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            var height = new Random().Next(0, 40);
            var y = dirtyRect.Height / 2 - (float)height / 2;
            //var x = (dirtyRect.Width / samples.Length) * i;
            canvas.FillColor = LineColor;
            canvas.FillRoundedRectangle(i, y, 5, height, 12);
        }

    }
}