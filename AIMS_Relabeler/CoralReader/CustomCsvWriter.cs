using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIMS_Relabeler.CoralReader.Model;

namespace AIMS_Relabeler.CoralReader
{
    class CustomCsvWriter
    {
        public void WriteCsv(IEnumerable<CoralPatch> collection)
        {
            var saveFileDialog1 = new SaveFileDialog
                                      {
                                          Filter = "csv files (*.csv)|*.csv",
                                          FilterIndex = 2,
                                          RestoreDirectory = true
                                      };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream myStream = saveFileDialog1.OpenFile();

                if (myStream.CanWrite)
                {
                    var file = new StreamWriter(myStream);

                    file.Write(CoralPatch.CsvHeader() + "\n");

                    foreach (var coralPatch in collection)
                        file.Write(coralPatch.ToCsvFormat() + "\n");

                    file.Close();
                }
            }
        }
    }
}
