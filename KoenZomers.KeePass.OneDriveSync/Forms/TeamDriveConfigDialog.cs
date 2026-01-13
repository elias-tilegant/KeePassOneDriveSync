using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomers.OneDrive.Api;
using KoenZomers.OneDrive.Api.Entities;
using KoenZomersKeePassOneDriveSync.Providers;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class TeamDriveConfigDialog : Form
    {
        private readonly OneDriveApi _oneDriveApi;
        private GraphSite[] _sites;
        private GraphDrive[] _drives;
        private string _currentPath = "";
        private List<string> _pathHistory = new List<string>();

        /// <summary>
        /// The selected Drive ID after configuration
        /// </summary>
        public string SelectedDriveId { get; private set; }

        /// <summary>
        /// The selected folder path within the drive
        /// </summary>
        public string SelectedPath { get; private set; }

        /// <summary>
        /// Display name for the configuration (Site / Drive name)
        /// </summary>
        public string SelectedDriveName { get; private set; }

        public TeamDriveConfigDialog(OneDriveApi oneDriveApi)
        {
            InitializeComponent();
            _oneDriveApi = oneDriveApi;
        }

        private async void TeamDriveConfigDialog_Load(object sender, EventArgs e)
        {
            await LoadSites();
        }

        private async Task LoadSites()
        {
            SitesComboBox.Items.Clear();
            SitesComboBox.Items.Add("Loading sites...");
            SitesComboBox.SelectedIndex = 0;
            SitesComboBox.Enabled = false;
            DrivesComboBox.Enabled = false;

            try
            {
                _sites = await GraphApiHelper.GetSites(_oneDriveApi);

                SitesComboBox.Items.Clear();
                SitesComboBox.Items.Add("-- Select a SharePoint Site --");

                foreach (var site in _sites.OrderBy(s => s.DisplayName))
                {
                    SitesComboBox.Items.Add(site.DisplayName ?? site.Name ?? site.Id);
                }

                SitesComboBox.SelectedIndex = 0;
                SitesComboBox.Enabled = true;
            }
            catch (Exception ex)
            {
                SitesComboBox.Items.Clear();
                SitesComboBox.Items.Add("Error loading sites");
                MessageBox.Show(
                    string.Format("Could not load SharePoint sites: {0}", ex.Message),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void SitesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SitesComboBox.SelectedIndex <= 0 || _sites == null)
            {
                DrivesComboBox.Items.Clear();
                DrivesComboBox.Enabled = false;
                FolderListView.Items.Clear();
                return;
            }

            var selectedSite = _sites.OrderBy(s => s.DisplayName).ElementAt(SitesComboBox.SelectedIndex - 1);
            await LoadDrives(selectedSite.Id);
        }

        private async Task LoadDrives(string siteId)
        {
            DrivesComboBox.Items.Clear();
            DrivesComboBox.Items.Add("Loading drives...");
            DrivesComboBox.SelectedIndex = 0;
            DrivesComboBox.Enabled = false;
            FolderListView.Items.Clear();

            try
            {
                _drives = await GraphApiHelper.GetSiteDrives(_oneDriveApi, siteId);

                DrivesComboBox.Items.Clear();
                DrivesComboBox.Items.Add("-- Select a Document Library --");

                foreach (var drive in _drives.OrderBy(d => d.Name))
                {
                    DrivesComboBox.Items.Add(drive.Name ?? drive.Id);
                }

                DrivesComboBox.SelectedIndex = 0;
                DrivesComboBox.Enabled = true;
            }
            catch (Exception ex)
            {
                DrivesComboBox.Items.Clear();
                DrivesComboBox.Items.Add("Error loading drives");
                MessageBox.Show(
                    string.Format("Could not load document libraries: {0}", ex.Message),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void DrivesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DrivesComboBox.SelectedIndex <= 0 || _drives == null)
            {
                FolderListView.Items.Clear();
                SelectedDriveId = null;
                return;
            }

            var selectedDrive = _drives.OrderBy(d => d.Name).ElementAt(DrivesComboBox.SelectedIndex - 1);
            SelectedDriveId = selectedDrive.Id;

            // Update display name
            var siteName = SitesComboBox.SelectedItem.ToString();
            var driveName = selectedDrive.Name ?? selectedDrive.Id;
            SelectedDriveName = string.Format("{0} / {1}", siteName, driveName);

            // Reset path and load root
            _currentPath = "";
            _pathHistory.Clear();
            await LoadFolderContents();
        }

        private async Task LoadFolderContents()
        {
            if (string.IsNullOrEmpty(SelectedDriveId))
            {
                return;
            }

            FolderListView.Items.Clear();
            CurrentPathTextBox.Text = string.IsNullOrEmpty(_currentPath) ? "/" : "/" + _currentPath;
            UpButton.Enabled = !string.IsNullOrEmpty(_currentPath);

            try
            {
                var items = await GraphApiHelper.GetDrivePathChildren(_oneDriveApi, SelectedDriveId, _currentPath);

                if (items == null || items.Collection == null)
                {
                    return;
                }

                // Sort: folders first, then by name
                var sortedItems = items.Collection
                    .OrderBy(i => i.Folder == null)
                    .ThenBy(i => i.Name);

                foreach (var item in sortedItems)
                {
                    var listViewItem = new ListViewItem
                    {
                        Text = item.Name,
                        Tag = item,
                        ImageKey = item.Folder != null ? "Folder" : "File"
                    };

                    FolderListView.Items.Add(listViewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Could not load folder contents: {0}", ex.Message),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            UpdateSelectedPath();
        }

        private async void FolderListView_DoubleClick(object sender, EventArgs e)
        {
            if (FolderListView.SelectedItems.Count == 0)
            {
                return;
            }

            var selectedItem = FolderListView.SelectedItems[0];
            var oneDriveItem = selectedItem.Tag as OneDriveItem;

            if (oneDriveItem == null || oneDriveItem.Folder == null)
            {
                return; // Only navigate into folders
            }

            // Navigate into folder
            _pathHistory.Add(_currentPath);
            _currentPath = string.IsNullOrEmpty(_currentPath)
                ? oneDriveItem.Name
                : _currentPath + "/" + oneDriveItem.Name;

            await LoadFolderContents();
        }

        private async void UpButton_Click(object sender, EventArgs e)
        {
            if (_pathHistory.Count > 0)
            {
                _currentPath = _pathHistory[_pathHistory.Count - 1];
                _pathHistory.RemoveAt(_pathHistory.Count - 1);
            }
            else
            {
                // Go to parent by removing last path segment
                var lastSlash = _currentPath.LastIndexOf('/');
                _currentPath = lastSlash > 0 ? _currentPath.Substring(0, lastSlash) : "";
            }

            await LoadFolderContents();
        }

        private void UpdateSelectedPath()
        {
            SelectedPath = _currentPath;
            SaveButton.Enabled = !string.IsNullOrEmpty(SelectedDriveId);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedDriveId))
            {
                MessageBox.Show(
                    "Please select a document library first.",
                    "Configuration Incomplete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FolderListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FolderListView_DoubleClick(sender, e);
            }
            else if (e.KeyCode == Keys.Back && UpButton.Enabled)
            {
                UpButton_Click(sender, e);
            }
        }

        private void UseCurrentFolderButton_Click(object sender, EventArgs e)
        {
            // Use the currently displayed folder (not navigate into it)
            UpdateSelectedPath();

            if (!string.IsNullOrEmpty(SelectedDriveId))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
