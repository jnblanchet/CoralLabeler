using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AIMS_Relabeler.CoralReader.Model;

namespace AIMS_Relabeler.CoralReader
{
    class AimsCsvReader : CoralCsvReader
    {
        public enum LabelFields // the labels represent the index in the AIMS format
        {
            SpecieDescription = 6,
            Genus = 7,
            FamilyDescription = 8,
            BenthosDescription = 9,
            GroupDescription = 10
        }

        private readonly LabelFields _labelType;

        public AimsCsvReader(LabelFields labelType)
        {
            _labelType = labelType;
        }

        public override ICollection<CoralPatch> ReadFile(StreamReader reader)
        {
            // read the header.
            reader.ReadLine();

            var lines = reader.ReadToEnd().Split('\n');
                    
            var coralPatches = new List<CoralPatch>(lines.Length);
            int i = 1;
            coralPatches.AddRange(from line in lines
                                  where line.Length > 0
                                  select line.Replace("\"", "").Split(',')
                                  into tokens
                                        let patchId = int.Parse(tokens.ElementAt(4))
                                        select new CoralPatch(tokens.ElementAt(0), i++)
                                                            {
                                                                ClassLabel = tokens.ElementAt((int) _labelType),
                                                                PatchCenterX = x[patchId - 1],
                                                                PatchCenterY = y[patchId-1],
                                                                IsRelativePosition = true
                                                            }
                                );
            return coralPatches;
        }

        // careful! the labels in matlab are inverted (y,x) instead of (x,y)
        private double[] x = { 0.25, 0.75, 0.50, 0.25, 0.75 };
        private double[] y = { 0.25, 0.25, 0.50, 0.75, 0.75 };

        public override string ToString()
        {
            return "AIMS (" + _labelType.ToString() + ")";
        }
    }
}
