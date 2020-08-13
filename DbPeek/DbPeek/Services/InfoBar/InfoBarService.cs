using DbPeek.Services.Notification;
using DbPeek.UserInterface;
using Microsoft;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DbPeek.Services.InfoBar
{
    internal class InfoBarService : IVsInfoBarUIEvents
    {
        private readonly IServiceProvider _serviceProvider;
        private uint _cookie;

        public static InfoBarService Instance { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            Instance = new InfoBarService(serviceProvider);
        }

        public InfoBarService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnClosed(IVsInfoBarUIElement infoBarUIElement)
        {
            infoBarUIElement.Unadvise(_cookie);
        }

        public void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            var context = Convert.ToString(actionItem.ActionContext);

            switch (context)
            {
                case nameof(HyperlinkCommands.Configure):
                    _ = infoBarUIElement.Unadvise(_cookie);
                    var configWindow = new ConfigurationControl();
                    configWindow.ShowDialog();
                    break;
                case nameof(HyperlinkCommands.Later):
                    _ = infoBarUIElement.Unadvise(_cookie);
                    break;
                case nameof(HyperlinkCommands.No):
                    NotificationService.PopMessage("Click Action", "You clicked No");
                    break;
                case nameof(HyperlinkCommands.Ok):
                    NotificationService.PopMessage("Click Action", "You clicked Ok");
                    break;
                case nameof(HyperlinkCommands.Yes):
                    NotificationService.PopMessage("Click Action", "You clicked Yes");
                    break;
                default:
                    //close the thingy by default
                    //or throw error maybe and display to the user???

                    _ = infoBarUIElement.Unadvise(_cookie);
                    break;
            }
        }

        public void ShowInfoBar(string message, params InfoBarHyperlink[] hyperLinkObjects)
        {
            if (_serviceProvider.GetService(typeof(SVsShell)) is IVsShell shell)
            {
                shell.GetProperty((int)__VSSPROPID7.VSSPROPID_MainWindowInfoBarHost, out var obj);
                var host = (IVsInfoBarHost)obj;

                if (host == null)
                {
                    return;
                }

                var text = new InfoBarTextSpan(message);

                var spans = new InfoBarTextSpan[] { text };

                var actions = new InfoBarActionItem[hyperLinkObjects.Length];
                for (int i = 0; i < hyperLinkObjects.Length; i++)
                {
                    actions[i] = hyperLinkObjects[i];
                }

                var infoBarModel = new InfoBarModel(spans, actions, KnownMonikers.StatusInformation, isCloseButtonVisible: true);

                var factory = _serviceProvider.GetService(typeof(SVsInfoBarUIFactory)) as IVsInfoBarUIFactory;
                Assumes.Present(factory);

                IVsInfoBarUIElement element = factory.CreateInfoBar(infoBarModel);
                element.Advise(this, out _cookie);
                host.AddInfoBar(element);
            }
        }
    }
}
