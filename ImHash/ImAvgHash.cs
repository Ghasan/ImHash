using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using SixLabors.ImageSharp.Processing.Transforms;

namespace ImHash
{
    public class ImAvgHash : IImHash
    {
        private const int WIDTH = 8;
        private const int HEIGHT = 8;

        private readonly int TOLERANCE;

        public ImAvgHash(int tolerance)
        {
            TOLERANCE = tolerance;
        }

        public bool[] GetImageHash(string path)
        {
            using (Image<Rgba32> image = Image.Load(path))
            {
                image.Mutate(x => x
                    .Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Crop,
                        Size = new SixLabors.Primitives.Size(WIDTH, HEIGHT)
                    })
                    .Grayscale());

                double avgLuminance = 0;

                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        avgLuminance += Luminance(image[i, j]);
                    }
                }

                avgLuminance /= WIDTH * HEIGHT;

                var hash = new bool[HEIGHT * WIDTH];

                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        hash[i * HEIGHT + j] = Luminance(image[i, j]) > avgLuminance;
                    }
                }

                return hash;
            }
        }

        public bool AreSimilar(bool[] firstHash, bool[] secondHash)
        {
            int length = WIDTH * HEIGHT;
            int distance = 0;

            for (int i = 0; i < length; i++)
            {
                if (firstHash[i] != secondHash[i])
                {
                    distance++;

                    if (distance > TOLERANCE)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Private

        private double Luminance(Rgba32 pixel)
        {
            return pixel.R * 0.21 + pixel.G * 0.72 + pixel.B * 0.07;
        }
    }
}
