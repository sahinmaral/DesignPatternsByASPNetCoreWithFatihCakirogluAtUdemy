using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WebApp.Adapter.Services;

public class AdvanceImageProcess : IAdvanceImageProcess
{
    public void AddWatermarkImage(Stream stream, string text, string filePath)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using Image originalImage = Image.Load<Rgba32>(stream);
        originalImage.Mutate(x => x.DrawText(text, SystemFonts.CreateFont("Arial", 12), Color.White, new PointF(30, 30)));
        
        originalImage.Save(filePath); 
    }
}