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
    public class Player : Crab
    {
        public int Score = 0;

        public void IncreaseScore(int points)
        {
            Score += points;
        }
    }
}
