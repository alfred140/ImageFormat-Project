using System;
using System.IO;
using System.Linq;

namespace lab3
{
    class Program
    {
        public static string search = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        static void Main(string[] args)
        {

            bool run = true;

            while (run)
            {
                Console.Write("type image name, ");
                Console.WriteLine("enter empty line to quit\n");

                byte[] pngFormat = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

                string imageName = Console.ReadLine();

                if (string.IsNullOrEmpty(imageName))
                {
                    break;
                }

                string fileName = search + @"\" + imageName;


                FileInfo info = new FileInfo(fileName);


                byte[] data;
                try
                {
                    data = new byte[info.Length];
                }
                catch
                {
                    Console.WriteLine("invalid format");
                    goto retry;
                }


                using (FileStream fs = info.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }

                if (data[0] == 'B' && data[1] == 'M')
                {
                    Console.Write("this is a .BMP image, ");

                    var width = data[18] + (data[19] * 256) +
                    (data[20] * 256 * 256) +
                    (data[21] * 256 * 256 * 256);

                    var height = data[22] + (data[23] * 256) +
                             (data[24] * 256 * 256) +
                             (data[25] * 256 * 256 * 256);

                    Console.WriteLine($"Resolution: {width}x{ height} pixels\n");
                }
                
               else if (data.Take(8).ToArray().SequenceEqual(pngFormat))
                {
                    Console.Write("this is a .PNG image, ");
                    Console.WriteLine(pngSize(fileName));
                }
                else
                {
                    Console.WriteLine("invalid format");
                }
            retry:;

            }
        }

        public static string pngSize(string fileName)

        {
            BinaryReader readSize = new BinaryReader(File.OpenRead(fileName));


            readSize.BaseStream.Position = 16;

            byte[] widthBytes = new byte[sizeof(int)];

            for (int i = 0; i < sizeof(int); i++)
            {
                widthBytes[sizeof(int) - 1 - i] = readSize.ReadByte();
            }


            byte[] heightbytes = new byte[sizeof(int)];

            for (int j = 0; j < sizeof(int); j++)
            {
                heightbytes[sizeof(int) - 1 - j] = readSize.ReadByte();
            }

            int width = BitConverter.ToInt32(widthBytes, 0);
            int height = BitConverter.ToInt32(heightbytes, 0);
            return $" Resolution: {width}x{height} pixels\n";
        }

    }
}
