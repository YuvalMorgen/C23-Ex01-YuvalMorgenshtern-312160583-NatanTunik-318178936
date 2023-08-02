using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FacebookWrapper;

// $G$ THE-001 (-13) Your grade on diagrams document - 87. Please see comments inside the document. (40% of your grade).
// $G$ RUL-004 (-20) Wrong zip name format / folder name format.
// $G$ RUL-004 (-10) Wrong doc file name. should be named as the solution.
// $G$ NTT-999 (-20) You should use C# 3.0 features.
// $G$ CSS-999 (-10) StyleCop errors.
// $G$ DSN-999 (-10) You should separate the logic and user interface into different classes.

namespace BasicFacebookFeatures
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Clipboard.SetText("design.patterns20cc");
            FacebookService.s_UseForamttedToStrings = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
