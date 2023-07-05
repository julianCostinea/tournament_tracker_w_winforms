﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TournamentTrackerLibrary.GlobalConfig.InitializeConnections(TournamentTrackerLibrary.DatabaseType.Sql);
            Application.Run(new CreatePrizeForm());
            Application.Run(new TournamentDashboardForm());
        }
    }
}
