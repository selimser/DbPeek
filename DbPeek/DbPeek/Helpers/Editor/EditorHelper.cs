using Microsoft.VisualStudio.TextManager.Interop;
using System;

namespace DbPeek.Helpers.Editor
{
    internal static class EditorHelper
    {
        private static IServiceProvider _serviceProvider;

        internal static void Initialise(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal static TextViewSelection GetSelection()
        {
            var service = _serviceProvider.GetService(typeof(SVsTextManager));
            var textManager = service as IVsTextManager2;
            _ = textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out IVsTextView view);

            view.GetSelection(out var startLine, out var startColumn, out var endLine, out var endColumn);
            var start = new TextViewPosition(startLine, startColumn);
            var end = new TextViewPosition(endLine, endColumn);

            view.GetSelectedText(out string selectedText);

            var selection = new TextViewSelection(start, end, selectedText);
            return selection;
        }
    }
}
