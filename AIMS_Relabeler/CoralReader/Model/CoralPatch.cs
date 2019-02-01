using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIMS_Relabeler.CoralReader.Model
{
    class CoralPatch
    {
        public string FilePath { private set; get; }
        public int UniqueId { private set; get; }

        public bool IsRejected = false;
        public double PatchCenterX, PatchCenterY; // If null, the patch was rejected.
        public int? x0, y0, x1, y1;
        public string ClassLabel;
        public bool IsRelativePosition = false; //Defines if the PatchCenterX,Y use relative or absolute coordinates.

        public CoralPatch(string imagePath, int uniqueId)
        {
            // builds the object using an image path, and an id
            // the other fields should be initialized through an initializer call {} (i.e. builder pattern)

            FilePath = imagePath;
            UniqueId = uniqueId;
        }

        public void ToAbsolute(int width, int height)
        {
            IsRelativePosition = false;
            PatchCenterX = PatchCenterX*width;
            PatchCenterY = PatchCenterY*height;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}@({2},{3}) from {4}", UniqueId, ClassLabel, PatchCenterX, PatchCenterY, FilePath);
        }

        public static string CsvHeader()
        {
            return String.Format("IsRejected,UniqueId,FilePath,PatchCenterX,PatchCenterY,x0,y0,x1,y1,ClassLabel");
        }

        public string ToCsvFormat()
        {
            return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", IsRejected, UniqueId, FilePath, PatchCenterX, PatchCenterY,
                safeInt(x0), safeInt(y0), safeInt(x1), safeInt(y1), ClassLabel);
        }

        private string safeInt(int? val)
        {
            if (val.HasValue)
                return val.ToString();
            return "";

        }

        public void ImportNewValues(CoralPatch other)
        {
            IsRelativePosition = other.IsRelativePosition;
            PatchCenterX = other.PatchCenterX;
            PatchCenterY = other.PatchCenterY;

            IsRejected = other.IsRejected;
            x0 = other.x0;
            x1 = other.x1;
            y0 = other.y0;
            y1 = other.y1;
        }
    }
}
