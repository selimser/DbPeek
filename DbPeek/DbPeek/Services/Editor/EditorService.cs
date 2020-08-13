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
            var start = new TextViewPosition(startLine, startColumn);
            var end = new TextViewPosition(endLine, endColumn);

            view.GetSelectedText(out var selectedText);

            var selection = new TextViewSelection(start, end, selectedText);
            return selection.Text;
        }

        public struct TextViewPosition
        {
            public int Line { get; }
            public int Column { get; }

            public TextViewPosition(int line, int column)
            {
                Line = line;
                Column = column;
            }

            public static bool operator <(TextViewPosition a, TextViewPosition b)
            {
                if (a.Line < b.Line)
                {
                    return true;
                }
                else if (a.Line == b.Line)
                {
                    return a.Column < b.Column;
                }
                else
                {
                    return false;
                }
            }

            public static bool operator >(TextViewPosition a, TextViewPosition b)
            {
                if (a.Line > b.Line)
                {
                    return true;
                }
                else if (a.Line == b.Line)
                {
                    return a.Column > b.Column;
                }
                else
                {
                    return false;
                }
            }

            public static TextViewPosition Min(TextViewPosition a, TextViewPosition b)
            {
                return a > b ? b : a;
            }

            public static TextViewPosition Max(TextViewPosition a, TextViewPosition b)
            {
                return a > b ? a : b;
            }
        }

        internal struct TextViewSelection
        {
            public TextViewPosition StartPosition { get; set; }
            public TextViewPosition EndPosition { get; set; }
            public string Text { get; set; }

            public TextViewSelection(TextViewPosition a, TextViewPosition b, string text)
            {
                StartPosition = TextViewPosition.Min(a, b);
                EndPosition = TextViewPosition.Max(a, b);
                Text = text;
            }
        }
    }
}
