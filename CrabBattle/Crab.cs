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
    public class Crab : Sprite
    {
        public double lastTimeShot = 0;
        public bool IsAlive = true;

        public static Texture2D ReadyImage;
        public static Texture2D StandbyImage;
        public static Texture2D DeadImage;

        public Crab() : base(Crab.StandbyImage)
        {
            var imageBounds = Image.Bounds;
            Rectangle = new Rectangle(0, 0, imageBounds.Width / 2, imageBounds.Height / 2);
            lastTimeShot = -1;
        }

        public bool CanShoot(GameTime time)
        {
            double currentTime = time.TotalGameTime.TotalSeconds;
            if (IsAlive && currentTime - lastTimeShot > 2)
            {
                return true;
            }
            return false;
        }

        public void HasShot(GameTime time)
        {
            lastTimeShot = time.TotalGameTime.TotalSeconds;
            Image = StandbyImage;
        }

        public void Die()
        {
            IsAlive = false;
            Image = DeadImage;
        }

        public void ResetShot()
        {
            lastTimeShot = 0;
        }
    }
}
