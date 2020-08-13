using Microsoft.VisualStudio.PlatformUI;

namespace DbPeek.UserInterface
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
