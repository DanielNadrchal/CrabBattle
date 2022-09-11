using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CrabBattle
{
    public class Sprite
    {
        public Texture2D Image { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool Visible { get; set; }

        public Sprite(Texture2D image)
        {
            Image = image;
            var imageBounds = image.Bounds;
            Rectangle = new Rectangle(0,0,imageBounds.Width, imageBounds.Height);
            Visible = true;
        }


    }
}
