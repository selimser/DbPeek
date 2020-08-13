using DbPeek.Helpers.Editor;
using DbPeek.Services.Database;
using DbPeek.Services.Editor;
using DbPeek.Services.InfoBar;
using DbPeek.Services.Notification;
using DbPeek.Services.Settings;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace DbPeek.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class PeekSpContextCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("60d43e44-102a-400c-a90b-09f71d120950");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeekSpContextCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private PeekSpContextCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);

            InfoBarService.Initialize((IServiceProvider)ServiceProvider);

        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static PeekSpContextCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in PeekSpSpanCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new PeekSpContextCommand(package, commandService);

            VsShellSettingsService.Initialise(package);
            NotificationService.Initialise(package);
            EditorService.Initialise(package);

        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            //TODO: Check if the extension is configured first.
            //If not, show an info bar with "Configure DB Strings"
            //Otherwise, carry on as usual.


            if (!VsShellSettingsService.ReadSetting<bool>("IsExtensionConfigured"))
            {
                InfoBarService.Instance.ShowInfoBar
                (
                    "Hi! Looks like DbPeek is initialised for the first time. Configure now?",
                    new InfoBarHyperlink("Configure", HyperlinkCommands.Configure),
                    new InfoBarHyperlink("Later", HyperlinkCommands.Later)
                );

                return;
            }

            //if everything's alright, capture the selected text.
            var capturedText = EditorService.GetSelection();
            if (!string.IsNullOrWhiteSpace(capturedText))
            {
                try
                {
                    var result = ThreadHelper.JoinableTaskFactory.Run(async delegate
                    {
                        return await SpUtilsService.Instance.GetStoredProcedureAsync(capturedText);
                    });

                    var dumpFile = FileService.CreateFileWithContents(result);
                    VsShellUtilities.OpenDocument((IServiceProvider)ServiceProvider, @dumpFile);
                }
                catch (Exception ex)
                {
                    NotificationService.PopMessage("Something went wrong!", ex.Message);
                }
            }
        }
    }
}
