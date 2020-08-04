using DbPeek.Helpers;
using System;
using System.Windows;
using WorkAlready;

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
            connectionStringInput.Text = SettingsHelper.ReadSetting<string>("TargetConnectionString");
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
                SettingsHelper.WriteSetting("TargetConnectionString", connectionStringInput.Text);

                if (!SettingsHelper.ReadSetting<bool>("IsExtensionConfigured"))
                {
                    SettingsHelper.WriteSetting("IsExtensionConfigured", true);
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.PopMessage
                (
                    "Error", 
                    $"An error occurred when saving the setting.\n{ex.Message}\n{ex.StackTrace}",
                    messageIcon: Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_WARNING
                );
            }

            Close();
        }
    }
}
