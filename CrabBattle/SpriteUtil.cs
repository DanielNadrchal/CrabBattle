using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrabBattle
{
    class SpriteUtil
    {
        public static int SpeedMultiplier = 5;

        public static void MoveSprite(Sprite sprite, int x, int y)
        {
            var curRec = sprite.Rectangle;
            curRec.X = x;
            curRec.Y = y;
            sprite.Rectangle = curRec;
        }

        public static void MoveSprite(Sprite sprite, Direction direction)
        {
            var curRec = sprite.Rectangle;
            if (direction.IsHorizontal())
            {
                int newX = direction == Direction.East ? curRec.X + SpeedMultiplier : curRec.X - SpeedMultiplier;
                curRec.X = newX;
            }
            else
            {
                int newY = direction == Direction.South ? curRec.Y + SpeedMultiplier : curRec.Y - SpeedMultiplier;
                curRec.Y = newY;
            }
            sprite.Rectangle = curRec;
        }

        public static void DrawSprite(Sprite sprite, SpriteBatch batch)
        {
            batch.Draw(sprite.Image, sprite.Rectangle, Color.White);
        }
    }
}
