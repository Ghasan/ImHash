using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using SixLabors.ImageSharp.Processing.Transforms;

namespace ImHash.Core
{
    public class ImDiffHash : IImHash
    {
        private const int WIDTH = 8;
        private const int HEIGHT = 8;

        private readonly int TOLERANCE;

        public ImDiffHash(int tolerance)
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

                var hash = new bool[HEIGHT * WIDTH];

                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            hash[0] = false;
                            continue;
                        }

                        if (j == 0)
                        {
                            hash[i * HEIGHT + j] = Luminance(image[i, j]) > Luminance(image[i - 1, WIDTH - 1]);
                        }
                        else
                        {
                            hash[i * HEIGHT + j] = Luminance(image[i, j]) > Luminance(image[i, j - 1]);
                        }
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
