using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace AIMS_Relabeler.Segmentation
{
    class SquareRegionSelector : RegionSelector
    {

        protected override void Draw()
        {
            DrawCenterMarker();

            // This will draw the patch boundaries
            if (SelectedPatch.x0.HasValue && SelectedPatch.x1.HasValue && SelectedPatch.y0.HasValue && SelectedPatch.y1.HasValue)
            {
                var r = new Rectangle(SelectedPatch.x0.Value, SelectedPatch.y0.Value,
                    SelectedPatch.x1.Value - SelectedPatch.x0.Value,
                    SelectedPatch.y1.Value - SelectedPatch.y0.Value);
                var c = SelectedPatch.IsRejected ? new Rgb(Color.DarkOrange) : new Rgb(Color.YellowGreen);
                DisplayedImage.Draw(r, c, 20);
            }
        }

        public override void ClickedOnImage(Point clickPosition)
        {
            // find out if x0 or x1 should move
            if (clickPosition.X<SelectedPatch.PatchCenterX)
                SelectedPatch.x0 = clickPosition.X;
            else if (clickPosition.X > SelectedPatch.PatchCenterX)
                SelectedPatch.x1 = clickPosition.X;

            // find out if y0 or y1 should move
            if (clickPosition.Y< SelectedPatch.PatchCenterY)
                SelectedPatch.y0 = clickPosition.Y;
            else if (clickPosition.Y > SelectedPatch.PatchCenterY)
                SelectedPatch.y1 = clickPosition.Y;
        }

        public override void Scrolled(double tickFactor)
        {
            
        }

        public SquareRegionSelector(ImageBox tagetToInvalidate) : base(tagetToInvalidate)
        {
        }
    }
}
