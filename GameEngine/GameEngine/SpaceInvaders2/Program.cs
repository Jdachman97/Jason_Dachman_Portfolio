﻿using System;

namespace SpaceInvaders2
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new SpaceInvaders2())
                game.Run();
        }
    }
#endif
}
