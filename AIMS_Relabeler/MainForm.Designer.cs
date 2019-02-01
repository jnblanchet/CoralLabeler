namespace AIMS_Relabeler
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MenuToolStrip = new System.Windows.Forms.ToolStrip();
            this.FormatToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.mergeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exportToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.skipToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SizeToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.CropAllStripButton = new System.Windows.Forms.ToolStripButton();
            this.SkipToNextStripButton = new System.Windows.Forms.ToolStripButton();
            this.menuSplitContainer = new System.Windows.Forms.SplitContainer();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.coralPatchLabelListbox = new System.Windows.Forms.ListBox();
            this.classDisplayLabel = new System.Windows.Forms.Label();
            this.CoralPictureBox = new Emgu.CV.UI.ImageBox();
            this.MenuToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuSplitContainer)).BeginInit();
            this.menuSplitContainer.Panel1.SuspendLayout();
            this.menuSplitContainer.Panel2.SuspendLayout();
            this.menuSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CoralPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuToolStrip
            // 
            this.MenuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FormatToolStripComboBox,
            this.openToolStripButton,
            this.mergeToolStripButton,
            this.exportToolStripButton,
            this.removeToolStripButton,
            this.skipToolStripButton,
            this.SizeToolStripTextBox,
            this.CropAllStripButton,
            this.SkipToNextStripButton});
            this.MenuToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuToolStrip.Name = "MenuToolStrip";
            this.MenuToolStrip.Size = new System.Drawing.Size(971, 25);
            this.MenuToolStrip.TabIndex = 0;
            // 
            // FormatToolStripComboBox
            // 
            this.FormatToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatToolStripComboBox.Name = "FormatToolStripComboBox";
            this.FormatToolStripComboBox.Size = new System.Drawing.Size(250, 25);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = global::AIMS_Relabeler.Properties.Resources.open;
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "Open CSV file";
            this.openToolStripButton.Click += new System.EventHandler(this.OpenToolStripButtonClick);
            // 
            // mergeToolStripButton
            // 
            this.mergeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mergeToolStripButton.Image = global::AIMS_Relabeler.Properties.Resources.merge;
            this.mergeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mergeToolStripButton.Name = "mergeToolStripButton";
            this.mergeToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.mergeToolStripButton.Text = "Combine progress with another previously exported file";
            this.mergeToolStripButton.Click += new System.EventHandler(this.MergeToolStripButtonClick);
            // 
            // exportToolStripButton
            // 
            this.exportToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportToolStripButton.Image = global::AIMS_Relabeler.Properties.Resources.export;
            this.exportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportToolStripButton.Name = "exportToolStripButton";
            this.exportToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.exportToolStripButton.Text = "Export current progress to new CSV file";
            this.exportToolStripButton.Click += new System.EventHandler(this.ExportToolStripButtonClick);
            // 
            // removeToolStripButton
            // 
            this.removeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeToolStripButton.Image = global::AIMS_Relabeler.Properties.Resources.remove;
            this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeToolStripButton.Name = "removeToolStripButton";
            this.removeToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.removeToolStripButton.Text = "toggle current patch rejection";
            this.removeToolStripButton.Click += new System.EventHandler(this.RemoveToolStripButtonClick);
            // 
            // skipToolStripButton
            // 
            this.skipToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.skipToolStripButton.Image = global::AIMS_Relabeler.Properties.Resources.skip;
            this.skipToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.skipToolStripButton.Name = "skipToolStripButton";
            this.skipToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.skipToolStripButton.Text = "Skip to next unchanged file";
            this.skipToolStripButton.Click += new System.EventHandler(this.SkipToolStripButtonClick);
            // 
            // SizeToolStripTextBox
            // 
            this.SizeToolStripTextBox.Name = "SizeToolStripTextBox";
            this.SizeToolStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.SizeToolStripTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SizeToolStripTextBoxKeyPress);
            // 
            // CropAllStripButton
            // 
            this.CropAllStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CropAllStripButton.Image = global::AIMS_Relabeler.Properties.Resources.crop;
            this.CropAllStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CropAllStripButton.Name = "CropAllStripButton";
            this.CropAllStripButton.Size = new System.Drawing.Size(23, 22);
            this.CropAllStripButton.Text = "Crop All";
            this.CropAllStripButton.Click += new System.EventHandler(this.CropAllStripButtonClick);
            // 
            // SkipToNextStripButton
            // 
            this.SkipToNextStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SkipToNextStripButton.Image = global::AIMS_Relabeler.Properties.Resources.next;
            this.SkipToNextStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SkipToNextStripButton.Name = "SkipToNextStripButton";
            this.SkipToNextStripButton.Size = new System.Drawing.Size(23, 22);
            this.SkipToNextStripButton.Text = "Skip To Next Image";
            this.SkipToNextStripButton.Click += new System.EventHandler(this.SkipToNextStripButtonClick);
            // 
            // menuSplitContainer
            // 
            this.menuSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menuSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.menuSplitContainer.Name = "menuSplitContainer";
            // 
            // menuSplitContainer.Panel1
            // 
            this.menuSplitContainer.Panel1.Controls.Add(this.searchTextBox);
            this.menuSplitContainer.Panel1.Controls.Add(this.coralPatchLabelListbox);
            // 
            // menuSplitContainer.Panel2
            // 
            this.menuSplitContainer.Panel2.Controls.Add(this.classDisplayLabel);
            this.menuSplitContainer.Panel2.Controls.Add(this.CoralPictureBox);
            this.menuSplitContainer.Size = new System.Drawing.Size(971, 516);
            this.menuSplitContainer.SplitterDistance = 300;
            this.menuSplitContainer.TabIndex = 1;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.searchTextBox.Location = new System.Drawing.Point(0, 494);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(298, 20);
            this.searchTextBox.TabIndex = 1;
            // 
            // coralPatchLabelListbox
            // 
            this.coralPatchLabelListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coralPatchLabelListbox.FormattingEnabled = true;
            this.coralPatchLabelListbox.Location = new System.Drawing.Point(0, 0);
            this.coralPatchLabelListbox.Name = "coralPatchLabelListbox";
            this.coralPatchLabelListbox.Size = new System.Drawing.Size(298, 514);
            this.coralPatchLabelListbox.TabIndex = 0;
            this.coralPatchLabelListbox.SelectedIndexChanged += new System.EventHandler(this.CoralPatchLabelListboxSelectedIndexChanged);
            // 
            // classDisplayLabel
            // 
            this.classDisplayLabel.AutoSize = true;
            this.classDisplayLabel.BackColor = System.Drawing.Color.Transparent;
            this.classDisplayLabel.Font = new System.Drawing.Font("Georgia", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classDisplayLabel.ForeColor = System.Drawing.Color.Gold;
            this.classDisplayLabel.Location = new System.Drawing.Point(4, 4);
            this.classDisplayLabel.Name = "classDisplayLabel";
            this.classDisplayLabel.Size = new System.Drawing.Size(0, 41);
            this.classDisplayLabel.TabIndex = 3;
            // 
            // CoralPictureBox
            // 
            this.CoralPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CoralPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CoralPictureBox.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.CoralPictureBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            this.CoralPictureBox.Location = new System.Drawing.Point(0, 0);
            this.CoralPictureBox.Name = "CoralPictureBox";
            this.CoralPictureBox.Size = new System.Drawing.Size(665, 514);
            this.CoralPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CoralPictureBox.TabIndex = 2;
            this.CoralPictureBox.TabStop = false;
            this.CoralPictureBox.Click += new System.EventHandler(this.CoralPictureBoxClick);
            this.CoralPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CoralPictureBox_MouseMove);
            this.CoralPictureBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CoralPictureBox_MouseWheel);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 541);
            this.Controls.Add(this.menuSplitContainer);
            this.Controls.Add(this.MenuToolStrip);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "AIMS Relabeler";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.MenuToolStrip.ResumeLayout(false);
            this.MenuToolStrip.PerformLayout();
            this.menuSplitContainer.Panel1.ResumeLayout(false);
            this.menuSplitContainer.Panel1.PerformLayout();
            this.menuSplitContainer.Panel2.ResumeLayout(false);
            this.menuSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuSplitContainer)).EndInit();
            this.menuSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CoralPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MenuToolStrip;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripComboBox FormatToolStripComboBox;
        private System.Windows.Forms.ToolStripButton mergeToolStripButton;
        private System.Windows.Forms.ToolStripButton exportToolStripButton;
        private System.Windows.Forms.SplitContainer menuSplitContainer;
        private System.Windows.Forms.ListBox coralPatchLabelListbox;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.ToolStripButton removeToolStripButton;
        private System.Windows.Forms.ToolStripButton skipToolStripButton;
        private System.Windows.Forms.ToolStripButton CropAllStripButton;
        private System.Windows.Forms.ToolStripButton SkipToNextStripButton;
        private System.Windows.Forms.ToolStripTextBox SizeToolStripTextBox;
        private Emgu.CV.UI.ImageBox CoralPictureBox;
        private System.Windows.Forms.Label classDisplayLabel;
    }
}

