
namespace CrabBattle.Sprites
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
