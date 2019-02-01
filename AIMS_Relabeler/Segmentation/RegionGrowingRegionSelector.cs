using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIMS_Relabeler.CoralReader.Model;
using AIMS_Relabeler.Segmentation.ImageProcessing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace AIMS_Relabeler.Segmentation
{
    class RegionGrowingRegionSelector : RegionSelector
    {
        private const int NumberBins = 10;
        private const double TextureScale = 0.25;
        private TextureMap _map;

        public override void CacheImage(Image<Rgb, byte> i)
        {
            if (CachedImage != null && i.Equals(CachedImage)) //skip if the image is the same.
                return;

            DisplayedImage = i;
            CachedImage = DisplayedImage.Copy();

            // cache texture representation
            _map = new TextureMap(i, NumberBins, TextureScale);
        }

        protected override void Draw()
        {
            if (!SelectedPatch.x0.HasValue || !SelectedPatch.y0.HasValue)
                return;

            Rectangle crop;
            var mask = _map.GetMask(SelectedPatch.x0.Value, SelectedPatch.y0.Value, out crop, GetPatchScale());
            mask = mask.Canny(new Gray(149),new Gray(255)).Dilate(5);

            // draw the mask on the image
            DisplayedImage.ROI = crop;
            for (int i = 0; i < mask.Rows; i++)
            {
                for (int j = 0; j < mask.Cols; j++)
                {
                    if ((float)mask[i, j].Intensity > 0)
                    {
                        DisplayedImage.Data[crop.Top+i, crop.Left + j, 0] = 255;
                        DisplayedImage.Data[crop.Top + i, crop.Left + j, 1] = 255;
                        DisplayedImage.Data[crop.Top + i, crop.Left + j, 2] = 0;
                        //new Rgb(Color.Yellow);
                    }
                }
            }
            DisplayedImage.ROI = Rectangle.Empty;

            DisplayedImage.Draw(crop,new Rgb(Color.Wheat),1 );

            // draw markers
            DrawMarker(SelectedPatch.x0.Value, SelectedPatch.y0.Value, new Rgb(Color.DodgerBlue));
            DrawCenterMarker();
        }

        public override void UpdatePatch(CoralPatch newPatch)
        {
            base.UpdatePatch(newPatch);
            // apply default parameters if needed
            if (!SelectedPatch.x0.HasValue)
                SelectedPatch.x0 = (int)SelectedPatch.PatchCenterX;
            if (!SelectedPatch.y0.HasValue)
                SelectedPatch.y0 = (int)SelectedPatch.PatchCenterY;
            if (!SelectedPatch.x1.HasValue)
                SetPatchScale(1.00d);
        }

        public override void ClickedOnImage(Point clickPosition)
        {
            //move new center
            SelectedPatch.x0 = clickPosition.X;
            SelectedPatch.y0 = clickPosition.Y;
        }

        public override void Scrolled(double tickFactor)
        {
            var newFactor = GetPatchScale() + tickFactor*0.01;

            if (newFactor > 3.0 || newFactor < 0.1) // bounded
                return;

            SetPatchScale(newFactor);
        }


        public RegionGrowingRegionSelector(ImageBox tagetToInvalidate) : base(tagetToInvalidate)
        {
        }

        // currentely, we use the same structure to encode the segmentation information (this avoids tons of code for multi-parameter support while exporting to CSV).
        private double GetPatchScale()
        {
            return (double)SelectedPatch.x1.Value/100;
        }
        private void SetPatchScale(double scale)
        {
            SelectedPatch.x1 = (int)(scale*100);
        }

    }
}
