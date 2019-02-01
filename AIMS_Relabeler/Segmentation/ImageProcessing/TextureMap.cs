using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace AIMS_Relabeler.Segmentation.ImageProcessing
{
    class TextureMap
    {
        private Image<Gray, float>[] _featureMap;
        private double _representationScaleFactor;
        private int _w, _h;
        private static readonly HueConverter HueConverter = new HueConverter();
        private static readonly LbpExtractor LbpExtractor = new LbpExtractor();

        public TextureMap(Image<Rgb, byte> image, int binCount, double representationScaleFactor)
        {
            var smallerImage = image.Resize(representationScaleFactor, INTER.CV_INTER_AREA);
            var normalizedImage = ComprehensiveColorImageNormalization.Apply(smallerImage);
            var hueImage = HueConverter.ToHue(normalizedImage);
            var colorMap = hueImage.ConvertScale<byte>(binCount - 1, 0);
            var lbpMap = LbpExtractor.ExtractLbpMap(smallerImage);

            _representationScaleFactor = representationScaleFactor;
            _featureMap = new Image<Gray, float>[binCount*2];
            _w = colorMap.Size.Width;
            _h = colorMap.Size.Height;

            var k = makeKernel();

            for (int i = 0; i < binCount; i++) //lbp
            {
                _featureMap[i] = lbpMap.Cmp(i, CMP_TYPE.CV_CMP_EQ).Convert<Gray, float>().Convolution(k);
            }
            for (int i = 0; i < binCount; i++) //color
            {
                _featureMap[binCount + i] = colorMap.Cmp(i, CMP_TYPE.CV_CMP_EQ).Convert<Gray, float>().Convolution(k);
            }
        }

        private ConvolutionKernelF makeKernel()
        {
            int r = 20; // circular neighborhood to consider
            int cx = r + 1, cy = r + 1, ix = r*2 + 1, iy = r*2 + 1, r2 = r*r;

            var mask = new float[ix, iy];

            for(int i=0; i<ix; i++)
                for (int j = 0; j < iy; j++)
                    mask[i, j] = (((i - cx)*(i - cx) + (j - cy)*(j - cy)) <= r2) ? 1.0f : 0.0f;
            return new ConvolutionKernelF(mask);
        }


        public Image<Gray, byte> GetMask(int x, int y, out Rectangle crop, double regionScaleFactor = 1.0d)
        {
            // get center
            int r = (int) (100*regionScaleFactor);
            x = (int) (x*_representationScaleFactor);
            y = (int) (y*_representationScaleFactor);

            // define weighted mask
            var weights = MakeGaussianSquareMask(r*2 + 1, r*1.5f);

            // get patch
            Rectangle rect = new Rectangle(x - r, y - r, r*2 + 1, r*2 + 1);
            int[] offset =
            {
                Math.Min(0, rect.Left), Math.Max(0, rect.Right - (_w-1)),
                Math.Min(0, rect.Top),  Math.Max(0, rect.Bottom - (_h-1))
            };
            rect.X -= offset[0];
            rect.Width -= offset[1] - offset[0] - 1;
            rect.Y -= offset[2];
            rect.Height -= offset[3] - offset[2] - 1;

            weights =
                weights.Copy(new Rectangle(-offset[0], -offset[2], weights.Width - offset[1] + offset[0], weights.Height - offset[3] + offset[2]));

            // get center feature reference
            //Image<Gray, float>[] featureMapPatch = new Image<Gray, float>[_featureMap.Length];
            //for (int i = 0; i < featureMapPatch.Length; i++)
            //    featureMapPatch[i] = _featureMap[i].Copy(rect);
            for (int i = 0; i < _featureMap.Length; i++)
                _featureMap[i].ROI = Rectangle.Empty;

            crop = new Rectangle((int)(rect.Left/_representationScaleFactor), (int)(rect.Top / _representationScaleFactor), (int)(rect.Width / _representationScaleFactor), (int)(rect.Height / _representationScaleFactor));

            // create similarity map (how similar are the pixels around)
            var refFeatures = _featureMap.Select(image => (float)image[y, x].Intensity).ToArray();
            var sqrdSumA = (float)Math.Sqrt(refFeatures.Select(f => f*f).Sum());
            
            for (int i = 0; i < _featureMap.Length; i++) // crop ROI
                _featureMap[i].ROI = rect;

            Image<Gray, float> segmentationDistanceMap = new Image<Gray, float>(weights.Size);
            for (int i = 0; i < weights.Rows; i++)
            {
                for (int j = 0; j < weights.Cols; j++)
                {
                    float sumAB = 0, sqrdSumB = 0;
                    for (int k = 0; k < _featureMap.Length; k++)
                    {
                        var v = (float)_featureMap[k][i, j].Intensity;
                        sumAB += refFeatures[k]*v;
                        sqrdSumB += v*v;
                    }
                    sqrdSumB = (float) Math.Sqrt(sqrdSumB);

                    segmentationDistanceMap[i, j] = new Gray((weights[i, j].Intensity * (sumAB / (sqrdSumA * sqrdSumB))));
                }
            }

            // normalize the map
            double[] minValue, maxValue; Point[] minLocation, maxLocation;
            segmentationDistanceMap.MinMax(out minValue, out maxValue, out minLocation, out maxLocation);
            segmentationDistanceMap = segmentationDistanceMap.Mul(1/maxValue.Max());

            //create binary map
            Image<Gray, byte> segmentationMap = new Image<Gray, byte>(weights.Size);
            for (int i = 5; i < weights.Rows- 5; i++)
            {
                for (int j = 5; j < weights.Cols- 5; j++)
                {
                    segmentationMap[i, j] = segmentationDistanceMap[i, j].Intensity > 0.9 ? new Gray(255) : new Gray(0);
                    //segmentationMap[i, j] = new Gray(255);
                }
            }

            // rescale, apply morphological correction, and return
            var rescaledImg = segmentationMap.Erode(5).Dilate(10).Erode(5).Resize(1.0d / _representationScaleFactor, INTER.CV_INTER_NN);
            return rescaledImg;
        }


        private Image<Gray, float> _lastMask;
        private int _lastSize = -1;
        private float _lastStd = -1;
        private Image<Gray, float> MakeGaussianSquareMask(int size, float std)
        {
            if (_lastSize == size && _lastStd == std)
                return _lastMask;

            _lastSize = size;
            _lastStd = std;

            _lastMask = new Image<Gray, float>(size, size);
            var c = ((size - 1)/2);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    var x = i - c;
                    var y = j - c;
                    _lastMask.Data[i, j, 0] = (float)Math.Exp(-(x*x + y*y)/(2*std*std));
                }

            return _lastMask;
        }


    }
}
