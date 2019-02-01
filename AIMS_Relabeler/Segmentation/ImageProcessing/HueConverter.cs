using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AIMS_Relabeler.Segmentation.ImageProcessing
{
    class HueConverter
    {
        private static double[] _lut;

        public HueConverter()
        {
            _lut = new double[256];
            for (int i=0;i< 256; i++)
            {
                _lut[i] = (double)i/255;
            }
        }

        public Image<Gray, double> ToHue(Image<Rgb, Byte> input)
        {
            var result = new Image<Gray, double>(input.Size);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Cols; j++)
                {
                    //Console.WriteLine(String.Format("===================="));
                    //Console.WriteLine(String.Format("pixels {0},{1}",i,j));
                    // mapping
                    var R = _lut[input.Data[i, j, 0]];
                    var G = _lut[input.Data[i, j, 1]];
                    var B = _lut[input.Data[i, j, 2]];
                    //Console.WriteLine(String.Format("triplet({0}  {1}  {2})", R, G, B));

                    // hue computation
                    if (B > G && G > R) // min = R, max = B
                        result.Data[i, j, 0] = 4.0 + (R - G) / (B - R);
                    else if (G > B && B > R) // min = R, max = G
                        result.Data[i, j, 0] = 2.0 + (B - R) / (G - R);
                    else if (R > G && G > B) // min = B, max = R
                        result.Data[i, j, 0] = (G - B) / (R - B);
                    else if (G > B && B > R) // min = B, max = G
                        result.Data[i, j, 0] = 2.0 + (B - R) / (G - B);
                    else if (B > R && R > G) // min = G, max = B
                        result.Data[i, j, 0] = 4.0 + (R - G) / (B - G);
                    else // min = G, max = R
                        result.Data[i, j, 0] = (G - B) / (R - G);

                    //Console.WriteLine(String.Format("before adjustement: {0}", result.Data[i, j, 0]));
                    // rescale + offset
                    if (result.Data[i, j, 0] < 0)
                        result.Data[i, j, 0] = 1 + (result.Data[i, j, 0] / 6);
                    else
                        result.Data[i, j, 0] = result.Data[i, j, 0] / 6;

                    //Console.WriteLine(String.Format("after adjustement: {0}", result.Data[i, j, 0]));
                }
            }

            return result;
        }
    }
}
