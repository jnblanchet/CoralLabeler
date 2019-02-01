using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIMS_Relabeler.CoralReader.Model;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace AIMS_Relabeler.Segmentation
{
    /// <summary>
    /// This class generates regions on interest based on the context.
    /// </summary>
    abstract class RegionSelector
    {
        private readonly ImageBox _tagetToUpdate;

        protected CoralPatch SelectedPatch;
        protected Image<Rgb, byte> CachedImage;
        protected Image<Rgb, byte> DisplayedImage;

        protected RegionSelector(ImageBox tagetToInvalidate)
        {
            _tagetToUpdate = tagetToInvalidate;
            CachedImage = null;
        }

        public void Redraw()
        {
            if (SelectedPatch != null)
            {
                CachedImage.CopyTo(DisplayedImage);
                Draw();
                _tagetToUpdate.Image = DisplayedImage;
            }
        }
        protected abstract void Draw();

        public virtual void CacheImage(Image<Rgb, byte> i)
        {
            DisplayedImage = i;

            CachedImage = DisplayedImage.Copy();
        }

        protected void DrawCenterMarker()
        {
            var c = SelectedPatch.IsRejected ? new Rgb(255, 0, 0) : new Rgb(0, 255, 0);
            DrawMarker((int)SelectedPatch.PatchCenterX, (int)SelectedPatch.PatchCenterY, c);
        }

        protected void DrawMarker(int x, int y, Rgb c)
        {
            var centerMark = new Rectangle(x - 10, y - 10, 20, 20);
            DisplayedImage.Draw(centerMark, new Rgb(0, 0, 0), 20);
            DisplayedImage.Draw(centerMark, c, 10);
        }

        public virtual void UpdatePatch(CoralPatch newPatch)
        {
            SelectedPatch = newPatch;
        }

        public abstract void ClickedOnImage(Point clickPosition);

        public abstract void Scrolled(double tickFactor);

    }
}
