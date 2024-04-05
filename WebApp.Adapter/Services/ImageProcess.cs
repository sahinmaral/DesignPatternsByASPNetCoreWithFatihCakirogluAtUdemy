using SkiaSharp;

namespace WebApp.Adapter.Services
{
    public class ImageProcess : IImageProcess
    {
        public void AddWatermark(string text, string filename, Stream imageStream)
        {
            if (imageStream == null)
            {
                throw new ArgumentNullException(nameof(imageStream));
            }

            try
            {
                imageStream.Position = 0;

                // Create SKBitmap from the imageStream
                using (var inputStream = new SKManagedStream(imageStream))
                using (var skImage = SKImage.FromEncodedData(inputStream))
                {
                    if (skImage == null)
                    {
                        throw new InvalidOperationException("Failed to decode the image.");
                    }

                    using (var skBitmap = SKBitmap.FromImage(skImage))
                    {
                        // Create SKCanvas to draw on the bitmap
                        using (var canvas = new SKCanvas(skBitmap))
                        {
                            // Create SKPaint for text
                            var paint = new SKPaint
                            {
                                Color = new SKColor(128, 255, 255, 255),
                                TextSize = 40,
                                IsAntialias = true,
                                TextAlign = SKTextAlign.Right,
                                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold,
                                    SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
                            };

                            // Calculate the text position
                            var textBounds = new SKRect();
                            paint.MeasureText(text, ref textBounds);
                            var x = 30;
                            var y = 30;

                            // Draw the text on the canvas
                            canvas.DrawText(text, x, y, paint);
                        }

                        // Save the modified bitmap to a file
                        using (var output = File.OpenWrite("wwwroot/watermarks/" + filename))
                        {
                            skBitmap.Encode(output, SKEncodedImageFormat.Jpeg, 100);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                Console.WriteLine($"Error adding watermark: {ex.Message}");
            }
        }
    }
}