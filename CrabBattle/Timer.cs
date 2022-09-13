using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace CrabBattle
{
    class Timer
    {
        private double lastTime = 0;
        private double CountTime;
        private double randomTime = 0;
        private Random random;

        public Timer(double countTime, Random Random)
        {
            CountTime = countTime;
            random = Random;
        }

        public bool Ready(GameTime time)
        {
            double currentTime = time.TotalGameTime.TotalSeconds;
            if (currentTime - lastTime > CountTime + randomTime)
            {
                lastTime = currentTime;

                if (random != null)
                {
                    CountTime = random.NextDouble();
                    CountTime = Math.Max(CountTime, ShortestRespawnTime());
                }

                return true;
            }
            return false;
        }

        private double ShortestRespawnTime()
        {
            return Math.Max(1 - (World.Level - 1) * 0.1, 0.4);
        }
    }
}
