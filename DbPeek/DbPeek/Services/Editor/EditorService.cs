using Microsoft.VisualStudio.TextManager.Interop;
using System;

namespace DbPeek.Services.Editor
{
    internal static class EditorService
    {
        private static IServiceProvider _serviceProvider;

        internal static void Initialise(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal static string GetSelection()
        {
            var service = _serviceProvider.GetService(typeof(SVsTextManager));
            var textManager = service as IVsTextManager2;
            _ = textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out IVsTextView view);

            view.GetSelection(out var startLine, out var startColumn, out var endLine, out var endColumn);
            var start = new EditorTextPosition(startLine, startColumn);
            var end = new EditorTextPosition(endLine, endColumn);

            view.GetSelectedText(out var selectedText);

            var selection = new EditorTextSelection(start, end, selectedText);
            return selection.Text;
        }

        internal struct EditorTextPosition
        {
            public int Row { get; }
            public int Column { get; }

            public EditorTextPosition(int row, int column)
            {
                Row = row;
                Column = column;
            }

            public static bool operator <(EditorTextPosition firstPosition, EditorTextPosition secondPosition)
            {
                if (firstPosition.Row < secondPosition.Row)
                {
                    return true;
                }
                else if (firstPosition.Row == secondPosition.Row)
                {
                    return firstPosition.Column < secondPosition.Column;
                }
                else
                {
                    return false;
                }
            }

            public static bool operator >(EditorTextPosition firstPosition, EditorTextPosition secondPosition)
            {
                if (firstPosition.Row > secondPosition.Row)
                {
                    return true;
                }
                else if (firstPosition.Row == secondPosition.Row)
                {
                    return firstPosition.Column > secondPosition.Column;
                }
                else
                {
                    return false;
                }
            }

            internal static EditorTextPosition Min(EditorTextPosition a, EditorTextPosition b)
            {
                return a > b ? b : a;
            }

            internal static EditorTextPosition Max(EditorTextPosition a, EditorTextPosition b)
            {
                return a > b ? a : b;
            }
        }

        internal struct EditorTextSelection
        {
            internal EditorTextPosition StartPosition { get; set; }
            internal EditorTextPosition EndPosition { get; set; }
            internal string Text { get; set; }

            public EditorTextSelection(EditorTextPosition a, EditorTextPosition b, string textContent)
            {
                StartPosition = EditorTextPosition.Min(a, b);
                EndPosition = EditorTextPosition.Max(a, b);
                Text = textContent;
            }
        }
    }
}
