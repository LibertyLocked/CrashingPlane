using System;

namespace CrashingPlane
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

#if DEBUG
            GlobalHelper.DebugMode = true;
#else
            if (args.Length >= 1 && args[0] == "debug"")
                GlobalHelper.DebugMode = true;
#endif

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

