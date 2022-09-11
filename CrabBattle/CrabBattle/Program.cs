using System;

namespace CrabBattle
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (CrabBattleGame game = new CrabBattleGame())
            {
                game.Run();
            }
        }
    }
}

