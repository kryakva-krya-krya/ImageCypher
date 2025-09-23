using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static ImageCypherLib.Cypher;

namespace ImageCypher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try {
                string message = Console.ReadLine();
                Bitmap bitmap = new Bitmap(Image.FromFile("img.jpg"));
                BitArray messageBits = new BitArray(Encoding.GetEncoding(1251).GetBytes(message));
                DebugBitArray(messageBits);
                Console.WriteLine("Cyphering...");
                bitmap = CypherImage(bitmap, messageBits);
                Console.WriteLine("Decyphering...");
                messageBits = DecypherImage(bitmap, true);
                DebugBitArray(messageBits);
                Console.WriteLine(EncodeBitArrayMessage(messageBits));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private static string EncodeBitArrayMessage(BitArray message)
        {
            int bitcounter = 0;
            string ret = "";
            List<bool> bitsBuffer = new List<bool>();
            foreach (bool bit in message)
            {
                bitcounter++;
                bitsBuffer.Add(bit);
                if (bitcounter >= 8)
                {
                    bitcounter = 0;
                    BitArray letter = new BitArray(8);
                    for (int i = 0; i < 8; i++)
                    {
                        letter[i] = bitsBuffer[0];
                        bitsBuffer.RemoveAt(0);
                    }
                    ret += Encoding.GetEncoding(1251).GetString(BitArrayToByteArray(letter));
                }
            }
            return ret;
        }

        private static void DebugBitArray(BitArray messageBits)
        {
            foreach (bool b in messageBits)
            {
                Console.Write(b ? 1 : 0);
            }
            Console.WriteLine();
        }
    }
}