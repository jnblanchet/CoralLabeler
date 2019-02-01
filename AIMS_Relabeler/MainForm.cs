using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AIMS_Relabeler.CoralReader;
using AIMS_Relabeler.CoralReader.Model;
using System.Linq;
using AIMS_Relabeler.Segmentation;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AIMS_Relabeler
{
    public partial class MainForm : Form
    {
        private Dictionary<string,CoralCsvReader> _dataReaders;
        private ICollection<CoralPatch> _coralPatches;
        private CoralPatch _selectedPatch;
        private CoralProxyImageReader _imageReader;
        private string _csvPath;
        private RegionSelector _segmentor;

        public MainForm()
        {
            InitializeComponent();
            classDisplayLabel.Parent = CoralPictureBox; //allows transparency
            classDisplayLabel.BackColor = Color.FromArgb(100,0,0,0);
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            // Initialize Readers and combo box
            _dataReaders = new Dictionary<string, CoralCsvReader>()
                               {
                                    { "CoralWatch custom", new CoralWatchCsvReader() },
                                   { "AIMS (Specie description)", new AimsCsvReader(AimsCsvReader.LabelFields.SpecieDescription) },
                                   { "AIMS (Genus)", new AimsCsvReader(AimsCsvReader.LabelFields.Genus) },
                                   { "AIMS (Family description)", new AimsCsvReader(AimsCsvReader.LabelFields.FamilyDescription) },
                                   { "AIMS (Benthos description)", new AimsCsvReader(AimsCsvReader.LabelFields.BenthosDescription) },
                                   { "AIMS (Group description)", new AimsCsvReader(AimsCsvReader.LabelFields.GroupDescription) },
                                   { "Relabeled File", new CustomCsvReader() },
                                   { "MLC", new MlcCsvReader() },
                               };

            foreach (var k in _dataReaders.Keys)
                FormatToolStripComboBox.Items.Add(k);
            FormatToolStripComboBox.SelectedIndex = 0;
        }

        private void OpenToolStripButtonClick(object sender, EventArgs e)
        {
            // select file
            var openFileDialog = new OpenFileDialog
                                      {
                                          Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*",
                                          FilterIndex = 1,
                                          Multiselect = false
                                      };
            // open file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileStream = openFileDialog.OpenFile();

                    _csvPath = Path.GetDirectoryName(openFileDialog.FileName);

                    // read file
                    _coralPatches =
                        _dataReaders[FormatToolStripComboBox.SelectedItem.ToString()].ReadFile(
                            new StreamReader(fileStream));

                    // populate listbox
                    coralPatchLabelListbox.Items.Clear();
                    foreach (var coral in _coralPatches)
                        coralPatchLabelListbox.Items.Add(coral.ToString());

                    _imageReader = new CoralProxyImageReader(_coralPatches, _csvPath);
                } catch(Exception x)
                {
                    MessageBox.Show("It seems there was an error reading the file. Make sure the right format is selected.");
                }
            }
            // segmentation stuff
            //_segmentor = new SquareRegionSelector(CoralPictureBox);
            // TODO: add a popup that asks which segmentation to use
            _segmentor = new RegionGrowingRegionSelector(CoralPictureBox);
        }

        private void CoralPatchLabelListboxSelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
                // update image and patch
                _selectedPatch = _coralPatches.ElementAt(coralPatchLabelListbox.SelectedIndex);
                var img = _imageReader.GetImage(coralPatchLabelListbox.SelectedIndex);
                CoralPictureBox.Image = img;
                _segmentor.CacheImage(img);
                _segmentor.UpdatePatch(_selectedPatch);

                // update coords if needed
                if (_selectedPatch.IsRelativePosition)
                    _selectedPatch.ToAbsolute(img.Width, img.Height);
            /*}
            catch(Exception x)
            {
                MessageBox.Show("The associated image file was not found! make sure the relative path is correct. The error was : " + x.Message);
            }*/

            // update display
            classDisplayLabel.Text = _selectedPatch.ClassLabel;
            DrawPatch();
        }

        private void DrawPatch()
        {
            // TODO: this code should belong to RegionSelectors, and it should be called by observing the model.
            /*try
            {*/
                _segmentor.Redraw();
            /*}catch(Exception x)
            {
                MessageBox.Show("A weird error happened while attempting to draw, try reloading the image, otherwise skip it for now...");
            }*/
            GC.Collect();
        }

        private void CoralPictureBoxClick(object sender, EventArgs e)
        {
            if (_selectedPatch == null)
                return;

            var me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                // transform point system
                var clickPosition = TranslateZoomMousePosition(me.X, me.Y, CoralPictureBox.Image.Size.Width,
                    CoralPictureBox.Image.Size.Height,
                    CoralPictureBox.Width, CoralPictureBox.Height);

                if (clickPosition.X == _selectedPatch.PatchCenterX || clickPosition.X == _selectedPatch.PatchCenterY)
                    return;

                _segmentor.ClickedOnImage(clickPosition);
                //CoralPictureBox.Invalidate();
                DrawPatch();
            }
        }

        private void CoralPictureBox_MouseWheel(object sender, EventArgs e)
        {
            if (_selectedPatch == null)
                return;

            var me = (MouseEventArgs)e;
            _segmentor.Scrolled((int)(me.Delta / 120)*5); // 120 is a microsoft standard, ~= 1 tick, 5 is a constant to tune wheel speed
            DrawPatch();

        }

        private void CoralPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CoralPictureBoxClick(sender, e);
            }
        }


        private float _cachedScaleFactor;
        protected Point TranslateZoomMousePosition(int pointX, int pointY, int imageW, int imageH, int containerW, int containerH)
        {
            var unscaledP = new Point(); 

            float imageRatio = imageW / (float)imageH; // image W:H ratio
            float containerRatio = containerW / (float)containerH; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                _cachedScaleFactor = containerW / (float)imageW;
                float scaledHeight = imageH * _cachedScaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(containerH - scaledHeight) / 2;
                unscaledP.X = (int)(pointX / _cachedScaleFactor);
                unscaledP.Y = (int)((pointY - filler) / _cachedScaleFactor);
            }
            else
            {
                // vertical image
                _cachedScaleFactor = containerH / (float)imageH;
                float scaledWidth = imageW * _cachedScaleFactor;
                float filler = Math.Abs(containerW - scaledWidth) / 2;
                unscaledP.X = (int)((pointX - filler) / _cachedScaleFactor);
                unscaledP.Y = (int)(pointY / _cachedScaleFactor);
            }
            return unscaledP;
        }

        private void RemoveToolStripButtonClick(object sender, EventArgs e)
        {
            RejectCurrentPatch();
        }

        private void RejectCurrentPatch()
        {
            if (_selectedPatch != null)
                _selectedPatch.IsRejected = !_selectedPatch.IsRejected;
            DrawPatch();
        }

        private void ExportToolStripButtonClick(object sender, EventArgs e)
        {
            Export();
        }

        private void Export()
        {
            if (_coralPatches != null && _coralPatches.Count > 1)
            {
                try
                {
                    var writer = new CustomCsvWriter();

                    writer.WriteCsv(_coralPatches.Where(p => !p.IsRelativePosition));
                }
                catch(Exception x)
                {
                    MessageBox.Show("There was an error while saving. Try saving to another file for now.");
                }
            }
        }

        private void MergeToolStripButtonClick(object sender, EventArgs e)
        {

            if(_coralPatches == null)
            {
                MessageBox.Show("A file must be opened first.");
                return;
            }

            // select file
            var openFileDialog = new OpenFileDialog
                                      {
                                          Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*",
                                          FilterIndex = 1,
                                          Multiselect = false
                                      };
            // open file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = openFileDialog.OpenFile();

                _csvPath = Path.GetDirectoryName(openFileDialog.FileName);

                // read file
                var newCoralPatches = (new CustomCsvReader()).ReadFile(new StreamReader(fileStream));

                // merge data
                foreach (var coral in newCoralPatches)
                {
                    _coralPatches.First(p => p.UniqueId == coral.UniqueId).ImportNewValues(coral);
                }
            }
        }

        // define hotkeys here
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // espace rejects the patch
            if (keyData == Keys.Escape)
            {
                RejectCurrentPatch();
                return true;
            }

            // space skips to the next patch
            if (keyData == Keys.Space)
            {
                if(coralPatchLabelListbox.SelectedIndex < coralPatchLabelListbox.Items.Count -1)
                    coralPatchLabelListbox.SelectedIndex++;
                return true;
            }

            // space skips to the next patch
            if (keyData == (Keys.Control | Keys.Space))
            {
                if (coralPatchLabelListbox.SelectedIndex > 0)
                    coralPatchLabelListbox.SelectedIndex--;
                return true;
            }

            // Ctrl + S pops the export menu
            if (keyData == (Keys.Control | Keys.S))
            {
                Export();
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SkipToolStripButtonClick(object sender, EventArgs e)
        {
            try
            {
                coralPatchLabelListbox.SelectedIndex =
                    _coralPatches.Skip(coralPatchLabelListbox.SelectedIndex).First(p => !p.IsRejected && !p.x0.HasValue)
                        .UniqueId - 1;
                coralPatchLabelListbox.Refresh();
            }catch (Exception)
            {
            }
        }

        private void CropAllStripButtonClick(object sender, EventArgs e)
        {
            int result = 0;
            if(int.TryParse(SizeToolStripTextBox.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out result))
            {
                int left = (int)Math.Ceiling((double)result/2);
                int right = (int)Math.Floor((double)result / 2);

                foreach(var p in _coralPatches)
                {
                    p.x0 = (int)p.PatchCenterX - left;
                    p.x1 = (int)p.PatchCenterX + right;
                    p.y0 = (int)p.PatchCenterY - left;
                    p.y1 = (int)p.PatchCenterY + right;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid integer size in the textbox (patch side length in pixels).");
            }
        }

        private void SizeToolStripTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            const char delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != delete;
        }

        private void SkipToNextStripButtonClick(object sender, EventArgs e)
        {
            int initialId = coralPatchLabelListbox.SelectedIndex+1;
            var currentPath = _coralPatches.ElementAt(coralPatchLabelListbox.SelectedIndex).FilePath;
            coralPatchLabelListbox.SelectedIndex = _coralPatches.Skip(coralPatchLabelListbox.SelectedIndex).First(p => p.FilePath.CompareTo(currentPath) != 0).UniqueId - 1;
            coralPatchLabelListbox.Refresh();

            var del =
                _coralPatches.Where(p => p.UniqueId >= initialId && p.UniqueId <= coralPatchLabelListbox.SelectedIndex);
            foreach (var coralPatch in del)
            {
                coralPatch.IsRejected = true;
            }
        }

    }
}