using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DbPeek.Services.Notification
{
    public static class NotificationService
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
