using DbPeek.Helpers.Editor;
using DbPeek.Services.Notification;
using DbPeek.Services.Settings;
using System;
using System.Windows;

namespace DbPeek.UserInterface
{
    /// <summary>
    /// Interaction logic for ConfigurationControl.xaml
    /// </summary>
    public partial class ConfigurationControl : BaseDialogWindow
    {
        public ConfigurationControl()
        {
            InitializeComponent();
            SetDefaultWindowProperties();
            SetExistingConnectionString();
            UpdateCacheFigures();
        }

        private void SetDefaultWindowProperties()
        {
            Title = "Configure DbPeek";
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen; //or center owner better? (vs centre)

#if DEBUG
            btnResetAllSettings_DEBUG.IsEnabled = true;
            btnResetAllSettings_DEBUG.Visibility = Visibility.Visible;
#endif

            
        }

        private void SetExistingConnectionString()
        {
            connectionStringInput.Text = VsShellSettingsService.ReadSetting<string>("TargetConnectionString");
        }

        private void UpdateCacheFigures()
        {
            FileCountLabel.Content = FileService.GetCacheFileCount();
            CacheTotalSizeLabel.Content = FileService.GetTotalCacheSize().AsFormatted();
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(connectionStringInput.Text))
            {
                MessageBox.Show("Connection string cannot be empty");
                return;
            }

            try
            {
                VsShellSettingsService.WriteSetting("TargetConnectionString", connectionStringInput.Text);

                if (!VsShellSettingsService.ReadSetting<bool>("IsExtensionConfigured"))
                {
                    VsShellSettingsService.WriteSetting("IsExtensionConfigured", true);
                }
            }
            catch (Exception ex)
            {
                NotificationService.PopMessage
                (
                    "Something went wrong!", 
                    $"An error occurred when saving the setting.\n{ex.Message}",
                    messageIcon: Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING
                );
            }

            Close();
        }

        private void ClearCache_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileService.ClearCache();
                UpdateCacheFigures();
            }
            catch (Exception ex)
            {
                NotificationService.PopMessage("Something went wrong!", $"{ex.Message}");
            }
        }
    }
}
