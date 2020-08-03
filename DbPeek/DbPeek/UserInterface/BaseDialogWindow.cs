using Microsoft.VisualStudio.PlatformUI;

namespace WorkAlready
{
    public class BaseDialogWindow : DialogWindow
    {
        public BaseDialogWindow()
        {
            HasMaximizeButton = false;
            HasMinimizeButton = false;
        }
    }
}
