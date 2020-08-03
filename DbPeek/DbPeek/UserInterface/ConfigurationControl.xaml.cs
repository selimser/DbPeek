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
        }

        private void SetDefaultWindowProperties()
        {
            Title = "Configure DbPeek";
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen; //or center owner better? (vs centre)
        }
    }
}
