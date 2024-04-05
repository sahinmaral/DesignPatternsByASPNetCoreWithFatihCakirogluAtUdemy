namespace WebApp.Adapter.Services;

public class AdvanceImageProcessDecorator(IAdvanceImageProcess imageProcess):IImageProcess
{
    public void AddWatermark(string text, string fileName, Stream imageStream)
    {
        imageProcess.AddWatermarkImage(imageStream,text,$"wwwroot/watermarks/{fileName}");
    }
}