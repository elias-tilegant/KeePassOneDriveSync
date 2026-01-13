namespace KoenZomersKeePassOneDriveSync.Forms
{
    partial class TeamDriveConfigDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SitesLabel = new System.Windows.Forms.Label();
            this.SitesComboBox = new System.Windows.Forms.ComboBox();
            this.DrivesLabel = new System.Windows.Forms.Label();
            this.DrivesComboBox = new System.Windows.Forms.ComboBox();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.FolderListView = new System.Windows.Forms.ListView();
            this.IconsList = new System.Windows.Forms.ImageList(this.components);
            this.CurrentPathTextBox = new System.Windows.Forms.TextBox();
            this.UpButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelDialogButton = new System.Windows.Forms.Button();
            this.UseCurrentFolderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // SitesLabel
            //
            this.SitesLabel.AutoSize = true;
            this.SitesLabel.Location = new System.Drawing.Point(12, 15);
            this.SitesLabel.Name = "SitesLabel";
            this.SitesLabel.Size = new System.Drawing.Size(175, 20);
            this.SitesLabel.TabIndex = 0;
            this.SitesLabel.Text = "Step 1: Select SharePoint Site";
            //
            // SitesComboBox
            //
            this.SitesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SitesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SitesComboBox.FormattingEnabled = true;
            this.SitesComboBox.Location = new System.Drawing.Point(12, 38);
            this.SitesComboBox.Name = "SitesComboBox";
            this.SitesComboBox.Size = new System.Drawing.Size(560, 28);
            this.SitesComboBox.TabIndex = 1;
            this.SitesComboBox.SelectedIndexChanged += new System.EventHandler(this.SitesComboBox_SelectedIndexChanged);
            //
            // DrivesLabel
            //
            this.DrivesLabel.AutoSize = true;
            this.DrivesLabel.Location = new System.Drawing.Point(12, 79);
            this.DrivesLabel.Name = "DrivesLabel";
            this.DrivesLabel.Size = new System.Drawing.Size(213, 20);
            this.DrivesLabel.TabIndex = 2;
            this.DrivesLabel.Text = "Step 2: Select Document Library";
            //
            // DrivesComboBox
            //
            this.DrivesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DrivesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DrivesComboBox.Enabled = false;
            this.DrivesComboBox.FormattingEnabled = true;
            this.DrivesComboBox.Location = new System.Drawing.Point(12, 102);
            this.DrivesComboBox.Name = "DrivesComboBox";
            this.DrivesComboBox.Size = new System.Drawing.Size(560, 28);
            this.DrivesComboBox.TabIndex = 3;
            this.DrivesComboBox.SelectedIndexChanged += new System.EventHandler(this.DrivesComboBox_SelectedIndexChanged);
            //
            // FolderLabel
            //
            this.FolderLabel.AutoSize = true;
            this.FolderLabel.Location = new System.Drawing.Point(12, 143);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(195, 20);
            this.FolderLabel.TabIndex = 4;
            this.FolderLabel.Text = "Step 3: Navigate to folder";
            //
            // FolderListView
            //
            this.FolderListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderListView.HideSelection = false;
            this.FolderListView.LargeImageList = this.IconsList;
            this.FolderListView.Location = new System.Drawing.Point(12, 199);
            this.FolderListView.MultiSelect = false;
            this.FolderListView.Name = "FolderListView";
            this.FolderListView.ShowItemToolTips = true;
            this.FolderListView.Size = new System.Drawing.Size(560, 280);
            this.FolderListView.SmallImageList = this.IconsList;
            this.FolderListView.TabIndex = 5;
            this.FolderListView.UseCompatibleStateImageBehavior = false;
            this.FolderListView.View = System.Windows.Forms.View.Tile;
            this.FolderListView.TileSize = new System.Drawing.Size(200, 40);
            this.FolderListView.DoubleClick += new System.EventHandler(this.FolderListView_DoubleClick);
            this.FolderListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FolderListView_KeyUp);
            //
            // IconsList
            //
            this.IconsList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.IconsList.ImageSize = new System.Drawing.Size(32, 32);
            this.IconsList.TransparentColor = System.Drawing.Color.Transparent;
            //
            // CurrentPathTextBox
            //
            this.CurrentPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentPathTextBox.BackColor = System.Drawing.Color.White;
            this.CurrentPathTextBox.Location = new System.Drawing.Point(12, 166);
            this.CurrentPathTextBox.Name = "CurrentPathTextBox";
            this.CurrentPathTextBox.ReadOnly = true;
            this.CurrentPathTextBox.Size = new System.Drawing.Size(490, 26);
            this.CurrentPathTextBox.TabIndex = 6;
            this.CurrentPathTextBox.Text = "/";
            //
            // UpButton
            //
            this.UpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpButton.Enabled = false;
            this.UpButton.Location = new System.Drawing.Point(508, 163);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(64, 32);
            this.UpButton.TabIndex = 7;
            this.UpButton.Text = "Up";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            //
            // SaveButton
            //
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(462, 493);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(110, 35);
            this.SaveButton.TabIndex = 8;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            //
            // CancelDialogButton
            //
            this.CancelDialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialogButton.Location = new System.Drawing.Point(346, 493);
            this.CancelDialogButton.Name = "CancelDialogButton";
            this.CancelDialogButton.Size = new System.Drawing.Size(110, 35);
            this.CancelDialogButton.TabIndex = 9;
            this.CancelDialogButton.Text = "Cancel";
            this.CancelDialogButton.UseVisualStyleBackColor = true;
            this.CancelDialogButton.Click += new System.EventHandler(this.CancelButton_Click);
            //
            // UseCurrentFolderButton
            //
            this.UseCurrentFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UseCurrentFolderButton.Location = new System.Drawing.Point(12, 493);
            this.UseCurrentFolderButton.Name = "UseCurrentFolderButton";
            this.UseCurrentFolderButton.Size = new System.Drawing.Size(160, 35);
            this.UseCurrentFolderButton.TabIndex = 10;
            this.UseCurrentFolderButton.Text = "Use This Folder";
            this.UseCurrentFolderButton.UseVisualStyleBackColor = true;
            this.UseCurrentFolderButton.Click += new System.EventHandler(this.UseCurrentFolderButton_Click);
            //
            // TeamDriveConfigDialog
            //
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialogButton;
            this.ClientSize = new System.Drawing.Size(584, 541);
            this.Controls.Add(this.UseCurrentFolderButton);
            this.Controls.Add(this.CancelDialogButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.CurrentPathTextBox);
            this.Controls.Add(this.FolderListView);
            this.Controls.Add(this.FolderLabel);
            this.Controls.Add(this.DrivesComboBox);
            this.Controls.Add(this.DrivesLabel);
            this.Controls.Add(this.SitesComboBox);
            this.Controls.Add(this.SitesLabel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "TeamDriveConfigDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Team Drive";
            this.Load += new System.EventHandler(this.TeamDriveConfigDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label SitesLabel;
        private System.Windows.Forms.ComboBox SitesComboBox;
        private System.Windows.Forms.Label DrivesLabel;
        private System.Windows.Forms.ComboBox DrivesComboBox;
        private System.Windows.Forms.Label FolderLabel;
        private System.Windows.Forms.ListView FolderListView;
        private System.Windows.Forms.ImageList IconsList;
        private System.Windows.Forms.TextBox CurrentPathTextBox;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelDialogButton;
        private System.Windows.Forms.Button UseCurrentFolderButton;
    }
}
