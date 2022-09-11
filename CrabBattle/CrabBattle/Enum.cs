using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrabBattle
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionExtensions
    {
        public static bool IsHorizontal(this Direction dir)
        {
            return dir == Direction.East || dir == Direction.West;
        }
    }
}
