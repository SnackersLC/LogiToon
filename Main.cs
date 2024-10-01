using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ToontownLGP
{
    internal class MainClass
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // Just for debbuging. Should be removed
            // AllocConsole();

            ToontownLGP.ToontownLGPApp app = new ToontownLGP.ToontownLGPApp();

            app.Run();
        }
    }
}