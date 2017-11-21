using System;

namespace PrivateBinCleaner
{
    internal class Program
    {
        private static readonly Logger Log = new Logger(typeof(Program));

        private static void Main()
        {
            try
            {
            Log.Info("Program starting.");
            var clean = new Cleaner();
            clean.Clean("config.xml");
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }
    }
}