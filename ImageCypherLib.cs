using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCypherLib
{
    public class ImageCypherLib
    {
        //Decode
        public static BitArray DecypherImage(Bitmap image, bool filterZeroBytes)
        {
            List<bool> bits = new List<bool>();
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    bits.AddRange(GetBitsFromPixel(image.GetPixel(j, i)));
                }
            }
            if (filterZeroBytes)
            bits = FilterZeroBytes(bits);
            return new BitArray(bits.ToArray());
        }

        private static List<bool> FilterZeroBytes(List<bool> bits)
        {
            List<bool> ret = new List<bool>();
            for (int i = 0; i < bits.Count; i += 8)
            {
                int sum = 0;
                for (int j = 0; j < 8; j++) if (i + j < bits.Count) sum += bits[i + j] ? 1 : 0;
                if (sum != 0) for (int j = 0; j < 8; j++) ret.Add(bits[i + j]);
                else break;
            }
            return ret;
        }

        private static bool[] GetBitsFromPixel(Color pixel)
        {
            return new bool[3] { new BitArray(new byte[] { pixel.R })[0], new BitArray(new byte[] { pixel.G })[0], new BitArray(new byte[] { pixel.B })[0] };
        }

        //Encode
        public static Bitmap CypherImage(Bitmap image, BitArray messageBits)
        {
            if (image.Width * image.Height * 3 < messageBits.Length) throw new Exception("Message lengtn bigger then image capacity");
            Color[] imagePixels = GetPixelsFromImage(image);
            int count = 0;
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    image.SetPixel(j, i, CypherPixel(image.GetPixel(j, i), messageBits, count));
                    count += 3;
                }
            }
            Console.WriteLine();
            return image;
        }

        private static Color CypherPixel(Color pixel, BitArray message, int counter)
        {
            BitArray pixelR = new BitArray(new byte[] { pixel.R });
            BitArray pixelG = new BitArray(new byte[] { pixel.G });
            BitArray pixelB = new BitArray(new byte[] { pixel.B });
            if (counter < message.Length)
                pixelR[0] = message[counter];
            else
                pixelR[0] = false;

            if (counter + 1 < message.Length)
                pixelG[0] = message[counter + 1];
            else
                pixelG[0] = false;

            if (counter + 2 < message.Length)
                pixelB[0] = message[counter + 2];
            else
                pixelB[0] = false;
            return Color.FromArgb(pixel.A, BitArrayToByteArray(pixelR)[0], BitArrayToByteArray(pixelG)[0], BitArrayToByteArray(pixelB)[0]);
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        private static Color[] GetPixelsFromImage(Bitmap image)
        {
            List<Color> ret = new List<Color>();
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    ret.Add(image.GetPixel(j, i));
                }
            }
            return ret.ToArray();
        }
    }


}
