using Microsoft;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DbPeek.Services.Shell
{
    public class StatusBarService
    {
        private readonly IServiceProvider _serviceProvider;
        private IVsStatusbar _statusBar;

        public static StatusBarService Instance { get; private set; }

        public StatusBarService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _statusBar = (IVsStatusbar)_serviceProvider.GetService(typeof(SVsStatusbar));
            Assumes.Present(_statusBar);
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            Instance = new StatusBarService(serviceProvider);
        }

        public void ShowMessage(string message)
        {
            _statusBar.IsFrozen(out var pfFrozen);
            if (pfFrozen != 0)
            {
                _statusBar.FreezeOutput(fFreeze: 0);
            }

            _statusBar.SetText(pszText: message);
            _statusBar.FreezeOutput(fFreeze: 1);
        }

        public void ClearMessage()
        {
            _statusBar.FreezeOutput(fFreeze: 0);
            _statusBar.Clear();
        }
    }
}
