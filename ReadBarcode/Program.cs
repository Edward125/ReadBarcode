using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace ReadBarcode
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string s = Console.ReadLine();
                if (s.ToLower() == "q")
                    break;
                else
                {
                    if (args.Length > 0)
                    {
                        if (File.Exists(args[0]))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            ScanBarCode(args[0]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("File '{0}' is not exists.", args[0]);
                            Console.ForegroundColor = ConsoleColor.White;

                        }
                    }
                }

               // Console.ReadLine();
            }
        }




        /// <summary>
        /// 条码识别
        /// </summary>
        private static  void ScanBarCode(string fileName)
        {
            DateTime now = DateTime.Now;
            Image primaryImage = Image.FromFile(fileName);

            Bitmap pImg = MakeGrayscale3((Bitmap)primaryImage);
            using (ZBar.ImageScanner scanner = new ZBar.ImageScanner())
            {
                scanner.SetConfiguration(ZBar.SymbolType.None, ZBar.Config.Enable, 0);
                scanner.SetConfiguration(ZBar.SymbolType.CODE39, ZBar.Config.Enable, 1);
                scanner.SetConfiguration(ZBar.SymbolType.CODE128, ZBar.Config.Enable, 1);

                List<ZBar.Symbol> symbols = new List<ZBar.Symbol>();
                symbols = scanner.Scan((Image)pImg);

                if (symbols != null && symbols.Count > 0)
                {
                    string result = string.Empty;
                    symbols.ForEach(s => result += "条码内容:" + s.Data + " 条码质量:" + s.Quality + Environment.NewLine);
                   // MessageBox.Show(result);
                    Console.WriteLine(result);
                }
            }
        }

        /// <summary>
        /// 处理图片灰度
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
