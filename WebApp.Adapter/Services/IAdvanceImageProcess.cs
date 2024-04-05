namespace WebApp.Adapter.Services
{
    public interface IAdvanceImageProcess
    {
        void AddWatermarkImage(Stream stream, string text, string filePath);
    }
}