namespace WebApp.Adapter.Services;

public interface IImageProcess
{
    void AddWatermark(string text, string fileName,Stream imageStream);
}