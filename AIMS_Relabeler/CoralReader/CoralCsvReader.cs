using System.Collections.Generic;
using System.IO;
using AIMS_Relabeler.CoralReader.Model;

namespace AIMS_Relabeler.CoralReader
{
    abstract class CoralCsvReader
    {
        public abstract ICollection<CoralPatch> ReadFile(StreamReader reader);
    }
}
