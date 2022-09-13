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
    class Enemy : Crab
    {
        private int MoveRandomMax = 100;
        private int MoveSwitchThreshold = 25;
        private int ShootRandomMax = 200;
        private int ShootThreshold = 1;

        public Enemy(bool lefter, Random random): base()
        {
            Random = random;

            int aiRoll = Random.Next(MoveRandomMax);

            if (aiRoll < MoveSwitchThreshold * 0.75)
            {
                aiRoll = Random.Next(MoveRandomMax);
                if (aiRoll < MoveSwitchThreshold * 2)
                    type = AiType.Center;
                else
                {
                    if (lefter)
                        type = AiType.Righter;
                    else
                        type = AiType.Lefter;
                }
            }
            else
            {
                if (lefter)
                    type = AiType.Lefter;
                else
                    type = AiType.Righter;
            }
        }

        Direction previousMove;
        AiType type;
        Random Random;

        public bool WillShoot(GameTime time)
        {
            if (!CanShoot(time))
                return false;

            return Random.Next(ShootRandomMax) < ShootThreshold;
        }

        public void Move()
        {
            Direction nextMove;

            if (type.Equals(AiType.Lefter))
                nextMove = Lefter();
            else if (type.Equals(AiType.Righter))
                nextMove = Righter();
            else
                nextMove = Center();

            previousMove = nextMove;

            SpriteUtil.MoveSprite(this, nextMove);
        }

        bool shouldRepeat;
        
        public Direction Lefter()
        {
            if (previousMove != Direction.South)
            {
                return Direction.South;
            }

            if (shouldRepeat)
            {
                shouldRepeat = false;
                return previousMove;
            }

            shouldRepeat = true;

            if (Random.Next(MoveRandomMax) < MoveSwitchThreshold)
                return Direction.East;

            return Direction.West;
        }

        public Direction Righter()
        {
            if (previousMove != Direction.South)
            {
                return Direction.South;
            }

            if (shouldRepeat)
            {
                shouldRepeat = false;
                return previousMove;
            }

            shouldRepeat = true;

            if (Random.Next(MoveRandomMax) < MoveSwitchThreshold)
                return Direction.West;

            return Direction.East;
        }

        public Direction Center()
        {
            if (previousMove != Direction.South)
            {
                return Direction.South;
            }

            if (Random.Next(MoveRandomMax) < 50)
                return Direction.West;

            return Direction.East;
        }

        private enum AiType{Lefter, Righter, Center};
    }
    
}
