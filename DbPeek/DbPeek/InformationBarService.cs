using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbPeek
{
    class InformationBarService : IVsInfoBarUIEvents
    {
        private readonly IServiceProvider _serviceProvider;
        private uint _cookie;

        public static InformationBarService Instance { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            Instance = new InformationBarService(serviceProvider);
        }

        public InformationBarService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnClosed(IVsInfoBarUIElement infoBarUIElement)
        {
            infoBarUIElement.Unadvise(_cookie);
        }

        private readonly AsyncPackage package;

        public void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            var context = Convert.ToString(actionItem.ActionContext);

            switch (context)
            {
                case nameof(HyperlinkCommands.Configure):
                    //run the Configure command

                    PopMessage("Click Action", "You clicked Configure");
                    break;
                case nameof(HyperlinkCommands.Later):
                    PopMessage("Click Action", "You clicked Later");
                    break;
                case nameof(HyperlinkCommands.No):
                    PopMessage("Click Action", "You clicked No");
                    break;
                case nameof(HyperlinkCommands.Ok):
                    PopMessage("Click Action", "You clicked Ok");
                    break;
                case nameof(HyperlinkCommands.Yes):
                    PopMessage("Click Action", "You clicked Yes");
                    break;
                default:
                    //close the thingy by default
                    //or throw error maybe and display to the user???
                    break;
            }
        }

        private void PopMessage(string title, string message, OLEMSGICON messageIcon = OLEMSGICON.OLEMSGICON_INFO, OLEMSGBUTTON messageButton = OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON defaultButton = OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST)
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


        public void ShowInfoBar(string message, params InfoBarHyperlink[] hyperLinkObjects)
        {
            var shell = _serviceProvider.GetService(typeof(SVsShell)) as IVsShell;
            if (shell != null)
            {
                shell.GetProperty((int)__VSSPROPID7.VSSPROPID_MainWindowInfoBarHost, out var obj);
                var host = (IVsInfoBarHost)obj;

                if (host == null)
                {
                    return;
                }
                var text = new InfoBarTextSpan(message);

                //var yes = new InfoBarHyperlink("Yes", "yes");
                //var no = new InfoBarHyperlink("No", "no");

                var spans = new InfoBarTextSpan[] { text };

                var actions = new InfoBarActionItem[hyperLinkObjects.Length];
                for (int i = 0; i < hyperLinkObjects.Length; i++)
                {
                    actions[i] = hyperLinkObjects[i];
                }

                var infoBarModel = new InfoBarModel(spans, actions, KnownMonikers.StatusInformation, isCloseButtonVisible: true);

                var factory = _serviceProvider.GetService(typeof(SVsInfoBarUIFactory)) as IVsInfoBarUIFactory;
                IVsInfoBarUIElement element = factory.CreateInfoBar(infoBarModel);
                element.Advise(this, out _cookie);
                host.AddInfoBar(element);
            }
        }
    }
}
