using System;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AIMS_Relabeler.Segmentation.ImageProcessing
{
    public static class ComprehensiveColorImageNormalization
    {
        public static Image<Rgb, byte> Apply(Image<Rgb, byte> input)
        {

            // init parameters
            var norm = (input.Width * input.Height); // normalization factor
            
            //init result
            var newImage = input.Copy().Convert<Rgb,double>() / 255;
            var R = newImage[0].Copy();
            var G = newImage[1].Copy();
            var B = newImage[2].Copy();
            var Ri = newImage[0];
            var Gi = newImage[1];
            var Bi = newImage[2];

            // get max value
            double[] minValue, maxValue; Point[] minLocation, maxLocation;
            newImage.MinMax(out minValue, out maxValue, out minLocation, out maxLocation);

            // looping criterion
            var criterion = maxValue.Max() / 100;
            double delta = 1;
            const int maxIter = 10;
            int i = 0;

            while (delta > criterion)
            {
                // First normalization
                var total = R + G + B + 0.000001;
                R = R.Mul(total.Pow(-1.0d));
                G = G.Mul(total.Pow(-1.0d));
                B = B.Mul(total.Pow(-1.0d));

                // Second normalization
                var Rtot = R.GetSum();
                var Gtot = G.GetSum();
                var Btot = B.GetSum();
                
                R = norm * R / Rtot.Intensity;
                G = norm * G / Gtot.Intensity;
                B = norm * B / Btot.Intensity;

                // Criterion
                double[] max = {0,0,0};
                Ri.AbsDiff(R).MinMax(out minValue, out maxValue, out minLocation, out maxLocation);
                max[0] = maxValue.Max();
                Gi.AbsDiff(G).MinMax(out minValue, out maxValue, out minLocation, out maxLocation);
                max[1] = maxValue.Max();
                Bi.AbsDiff(B).MinMax(out minValue, out maxValue, out minLocation, out maxLocation);
                max[2] = maxValue.Max();
                delta = max.Max();
                //Console.WriteLine(String.Format("i={1}, delta={0}",delta,i));

                R.CopyTo(Ri);
                G.CopyTo(Gi);
                B.CopyTo(Bi);

                if (i++ > maxIter)
                {
                    break;
                }
            }
            newImage[0] = R;
            newImage[1] = G;
            newImage[2] = B;
            return newImage.Mul(255).Convert<Rgb, byte>();
        }
    }
}
