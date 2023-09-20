using NAudio.Wave;

namespace CA.Maui.Components;

public class WaveformGraphicsView : GraphicsView
{
    public string Path { get => (string)GetValue(PathProperty); set => SetValue(PathProperty, value); }
    public static readonly BindableProperty PathProperty = BindableProperty.Create(
        nameof(Path), typeof(string), typeof(WaveformGraphicsView), propertyChanged: PathPropertyChanged);

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
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (Path is null) return;
        var reader = new AudioFileReader(Path);
        if (File.Exists(Path))
        {
            var width = (int)dirtyRect.Width;
            var height = (int)dirtyRect.Height;
            const int bufferSize = 10;
            var buffer = new byte[bufferSize];
            using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = 0;
                while ((bytesRead = fs.Read(buffer, 0, bufferSize)) > 0)
                {
                    var sampleValue = buffer[0] / 128.0f - 1.0f;
                    var halfHeight = height / 2;
                    var y = (int)(halfHeight + sampleValue * halfHeight);
                    canvas.StrokeSize = 2;
                    canvas.DrawLine(0, halfHeight, 10, y);
                }
            }
        }
        
        //for (var x = 0; x < width; x++)
        //{
        //    var position = x * reader.Length / width;
        //    reader.Seek(position, SeekOrigin.Begin);
        //    var bytesRead = reader.Read(buffer, 0, 1);
        //    if (bytesRead == 0)
        //        break;
        //    var sampleValue = buffer[0] / 128.0f - 1.0f;
        //    var halfHeight = height / 2;
        //    var y = (int)(halfHeight + sampleValue * halfHeight);
        //    canvas.StrokeSize = 2;
        //    canvas.DrawLine(x, halfHeight, x, y);
        //}
    }
}