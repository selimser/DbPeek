using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DbPeek.Helpers
{
    public static class NotificationHelper
    {
        private static IServiceProvider _serviceProvider;
        
        internal static void Initialise(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal static void PopMessage(string title, string message, OLEMSGICON messageIcon = OLEMSGICON.OLEMSGICON_INFO, OLEMSGBUTTON messageButton = OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON defaultButton = OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST)
        {
            VsShellUtilities.ShowMessageBox
            (
               _serviceProvider,
               message,
               title,
               messageIcon,
               messageButton,
               defaultButton
            );
        }
    }
}
