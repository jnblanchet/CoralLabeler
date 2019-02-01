using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AIMS_Relabeler.CoralReader.Model;

namespace AIMS_Relabeler.CoralReader
{
    class CustomCsvReader : CoralCsvReader
    {
        public override ICollection<CoralPatch> ReadFile(StreamReader reader)
        {
            // read the header.
            reader.ReadLine();

            var lines = reader.ReadToEnd().Split('\n');
                    
            var coralPatches = new List<CoralPatch>(lines.Length);

            coralPatches.AddRange(from line in lines
                                  where line.Length > 0
                                  select line.Replace("\"", "").Split(',')
                                  into tokens
                                        let patchId = int.Parse(tokens.ElementAt(4))
                                        select new CoralPatch(tokens.ElementAt(2), int.Parse(tokens.ElementAt(1)))
                                                            {
                                                                IsRejected = Boolean.Parse(tokens.ElementAt(0)),
                                                                ClassLabel = tokens.ElementAt(9),
                                                                PatchCenterX = int.Parse(tokens.ElementAt(3)),
                                                                PatchCenterY = int.Parse(tokens.ElementAt(4)),
                                                                x0 = tokens.ElementAt(5).Length > 0 ? (int?)int.Parse(tokens.ElementAt(5)) : null,
                                                                y0 = tokens.ElementAt(6).Length > 0 ? (int?)int.Parse(tokens.ElementAt(6)) : null,
                                                                x1 = tokens.ElementAt(7).Length > 0 ? (int?)int.Parse(tokens.ElementAt(7)) : null,
                                                                y1 = tokens.ElementAt(8).Length > 0 ? (int?)int.Parse(tokens.ElementAt(8)) : null
                                                            }
                                );
            return coralPatches;
        }
        public override string ToString()
        {
            return "AIMS_Relabeler csv";
        }
    }
}
