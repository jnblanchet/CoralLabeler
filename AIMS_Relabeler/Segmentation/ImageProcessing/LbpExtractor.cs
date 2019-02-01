using Emgu.CV;
using Emgu.CV.Structure;

namespace AIMS_Relabeler.Segmentation.ImageProcessing
{
    class LbpExtractor
    {
        private const int Sampling = 8, Distance = 1; // cannot be customized for now, this simplifies the implementation
        private readonly byte[] _mapping;
        public LbpExtractor()
        {
            //create riu2 mapping
            const int max = 1 << Sampling;
            _mapping = new byte[max];

            for (int i = 0; i < max; i++)
            {
                var j = (i << 1) | (i >> 31);
                var diff = (i ^ j);

                int c;
                for (c = 0; diff!=0; diff >>= 1)
                    c += diff & 1;

                if (c <= 2)
                    for (var ii = i; ii != 0; ii >>= 1)
                        _mapping[i] += (byte)(ii & 1);
                else
                    _mapping[i] = Sampling + 1;
            }
        }

        public Image<Gray, byte> ExtractLbpMap(Image<Rgb, byte> input)
        {
            var lbpMap = new Image<Gray, byte>(input.Size);

            for (int i = 1; i < input.Rows-1; i++)
            {
                for (int j = 1; j < input.Cols-1; j++)
                {
                    var center = input.Data[i, j, 0];
                    var code = ((input.Data[i - 1, j - 1, 0] >= center) ? 0x80 : 0)
                               | ((input.Data[i, j - 1, 0] >= center) ? 0x40 : 0)
                               | ((input.Data[i + 1, j - 1, 0] >= center) ? 0x20 : 0)
                               | ((input.Data[i - 1, j, 0] >= center) ? 0x10 : 0)
                               | ((input.Data[i + 1, j, 0] >= center) ? 0x8 : 0)
                               | ((input.Data[i - 1, j + 1, 0] >= center) ? 0x4 : 0)
                               | ((input.Data[i, j + 1, 0] >= center) ? 0x2 : 0)
                               | ((input.Data[i + 1, j + 1, 0] >= center) ? 0x1 : 0);
                    lbpMap.Data[i, j, 0] = _mapping[code];
                }
            }

            return lbpMap;
        }
    }
}
