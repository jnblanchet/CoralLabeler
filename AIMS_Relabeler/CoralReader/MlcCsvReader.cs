using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AIMS_Relabeler.CoralReader.Model;

namespace AIMS_Relabeler.CoralReader
{
    class MlcCsvReader : CoralCsvReader
    {
        public override ICollection<CoralPatch> ReadFile(StreamReader reader)
        {
            // read the header.
            reader.ReadLine();

            var lines = reader.ReadToEnd().Split('\n');
                    
            var coralPatches = new List<CoralPatch>(lines.Length);
            int i = 1;
            coralPatches.AddRange(from line in lines
                                  where line.Length > 0
                                  select line.Split(',')
                                  into tokens
                                        select new CoralPatch(tokens.ElementAt(0), i++)
                                                            {
                                                                ClassLabel = tokens.ElementAt(3),
                                                                PatchCenterX = int.Parse(tokens.ElementAt(2)),
                                                                PatchCenterY = int.Parse(tokens.ElementAt(1))
                                                            }
                                );
            return coralPatches;
        }

        public override string ToString()
        {
            return "MLC Processed File";
        }
    }
}
