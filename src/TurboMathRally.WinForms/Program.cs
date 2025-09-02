using TurboMathRally.WinForms;

namespace TurboMathRally.WinForms
{
    /// <summary>
    /// Main entry point for the Windows Forms version of Turbo Math Rally
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            // Show the main form
            Application.Run(new MainMenuForm());
        }
    }
}
