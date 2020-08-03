using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        }

        private void SetDefaultWindowProperties()
        {
            Title = "Configure DbPeek";
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen; //or center owner better? (vs centre)
        }

        private void rbDbManual_Checked(object sender, RoutedEventArgs e)
        {
            SetMode(ConnectionEditingMode.Manual);
        }

        private void rbDbConnectionString_Checked(object sender, RoutedEventArgs e)
        {
            SetMode(ConnectionEditingMode.ConnectionString);
        }

        private void SetMode(ConnectionEditingMode editingMode)
        {
            switch (editingMode)
            {
                case ConnectionEditingMode.Manual:
                    serverInput.IsEnabled = true;
                    dbInput.IsEnabled = true;
                    userIdInput.IsEnabled = true;
                    passwordInput.IsEnabled = true;
                    integratedSecurityInput.IsEnabled = true;
                    connectionStringInput.IsEnabled = false;
                    break;
                default:
                case ConnectionEditingMode.ConnectionString: //fallback to this
                    serverInput.IsEnabled = false;
                    dbInput.IsEnabled = false;
                    userIdInput.IsEnabled = false;
                    passwordInput.IsEnabled = false;
                    integratedSecurityInput.IsEnabled = false;
                    connectionStringInput.IsEnabled = true;
                    break;
            }

            if (CanGenerateConnectionString())
            {
                SetConnectionStringToInput();
            }
        }

        private string BuildConnectionString()
        {
            var connectionBuilder = new StringBuilder();
            connectionBuilder.Append($"server={serverInput.Text};");
            connectionBuilder.Append($"database={serverInput.Text};");

            if (UseIntegratedSecurity())
            {
                connectionBuilder.Append("integrated security=true;");
            }
            else
            {
                connectionBuilder.Append($"user id={userIdInput.Text};");
                connectionBuilder.Append($"password={passwordInput.Text};");
            }

            return connectionBuilder.ToString();
        }

        private bool UseIntegratedSecurity()
        {
            return integratedSecurityInput.IsChecked ?? false;
        }

        private void integratedSecurityInput_Checked(object sender, RoutedEventArgs e)
        {
            if (UseIntegratedSecurity())
            {
                userIdInput.IsEnabled = false;
                passwordInput.IsEnabled = false;
            }
            else
            {
                userIdInput.IsEnabled = true;
                passwordInput.IsEnabled = true;
            }
        }

        private void FormControls_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CanGenerateConnectionString())
            {
                SetConnectionStringToInput();
            }
        }

        private void SetConnectionStringToInput()
        {
            var builtConnectionString = BuildConnectionString();
            connectionStringInput.Text = builtConnectionString;
        }

        private bool CanGenerateConnectionString()
        {
            /*
             Conditions to generate connection string on LostFocus:
                a. All 4 text boxes are filled with something
                b. Server and Database filled in + WinAuth is checked
             */

            //temporary logic, needs improvement.

            var canGenerateConnectionString = false;
            if (UseIntegratedSecurity())
            {
                canGenerateConnectionString = HasValue(serverInput) && HasValue(dbInput);
            }
            else
            {
                canGenerateConnectionString = HasValue(serverInput) && HasValue(dbInput) && HasValue(userIdInput) && HasValue(passwordInput);
            }

            return canGenerateConnectionString;

            bool HasValue(TextBox target)
            {
                if (!string.IsNullOrWhiteSpace(target.Text))
                {
                    return true;
                }

                return false;
            }
        }


    }
}
