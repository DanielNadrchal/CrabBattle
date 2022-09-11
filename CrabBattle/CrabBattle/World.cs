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
    class World
    {
        public static int Level = 1;

        public Player player;
        public Sprite bullet;
        public Sprite cave;
        public Sprite water;
        public List<Enemy> enemies;
        public List<Enemy> dieingEnemies;
        public List<Enemy> deadEnemies;
        public SpriteFont font;
        public Vector2 scoreFontPos;
        public Vector2 escapeFontPos;
        public Vector2 thirdFontPos;
        public Vector2 fourthFontPos;

        public List<Sprite> enemyBullets;

        public int ScreenHeight;
        
        private Timer enemySpawnTimer;
        private Random random;

        public int enemyPoints = 5;
        public double enemySpawnTime = 1;

        private int EscapeCount = 0;
        private int EnemyKilledCount = 0;
        private int EnemyKilledCountTillNextLevel = 20;

        public World()
        {
            enemies = new List<Enemy>();
            enemyBullets = new List<Sprite>();
            random = new Random();
            enemySpawnTimer = new Timer(enemySpawnTime, random);
        }

        public void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteUtil.DrawSprite(cave, spriteBatch);
            SpriteUtil.DrawSprite(water, spriteBatch);

            if (player.Visible)
            {
                SpriteUtil.DrawSprite(player, spriteBatch);
                if (player.CanShoot(gameTime))
                {
                    player.Image = Crab.ReadyImage;
                }
            }

            if (bullet.Visible)
                SpriteUtil.DrawSprite(bullet, spriteBatch);

            foreach (var enemy in enemies)
            {
                SpriteUtil.DrawSprite(enemy, spriteBatch);
                if (enemy.CanShoot(gameTime))
                {
                    enemy.Image = Crab.ReadyImage;
                }
            }

            foreach (var enemy in dieingEnemies)
            {
                SpriteUtil.DrawSprite(enemy, spriteBatch);
            }

            foreach (var enemy in deadEnemies)
            {
                SpriteUtil.DrawSprite(enemy, spriteBatch);
            }

            foreach (var enemyBullet in enemyBullets)
            {
                SpriteUtil.DrawSprite(enemyBullet, spriteBatch);
            }

            string score = "Score: " + player.Score.ToString();
            Vector2 fontOrigin = font.MeasureString(score) / 2;
            spriteBatch.DrawString(font, score, scoreFontPos, Color.LightGreen, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            string escape = "Escaped: " + EscapeCount.ToString();
            fontOrigin = font.MeasureString(escape) / 2;
            spriteBatch.DrawString(font, escape, escapeFontPos, Color.LightGreen, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            string level = "Level: " + Level.ToString();
            fontOrigin = font.MeasureString(level) / 2;
            spriteBatch.DrawString(font, level, thirdFontPos, Color.LightGreen, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            //string toNextLevelString = "Next Level: " + EnemyKilledCountTillNextLevel.ToString();
            //fontOrigin = font.MeasureString(toNextLevelString) / 2;
            //spriteBatch.DrawString(font, toNextLevelString, fourthFontPos, Color.LightGreen, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

        }

        public void UpdateWorld(GameTime time, KeyboardState keyboardState)
        {
            RemoveDead(time);
            HandleInput(time, keyboardState);
            UpdateBullet(time);
            EnemyPlot(time);
            UpdateEnemyBullets(time);
        }

        private void RemoveDead(GameTime time)
        {
            deadEnemies = dieingEnemies;
            dieingEnemies = enemies.Where(a => !a.IsAlive).ToList();
            enemies = enemies.Where(a => a.IsAlive && a.Rectangle.Y < ScreenHeight).ToList();
        }

        private void HandleInput(GameTime time, KeyboardState keyboardState)
        {
            if (!player.IsAlive)
            {
                if (keyboardState.IsKeyDown(Keys.D1)  || keyboardState.IsKeyDown(Keys.D5) )
                {
                    RevivePlayer();
                }
                else
                {
                    return;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                SpriteUtil.MoveSprite(player, Direction.West);
            }
            if (keyboardState.IsKeyDown(Keys.Right)  || keyboardState.IsKeyDown(Keys.D))
            {
                SpriteUtil.MoveSprite(player, Direction.East);
            }
            if (keyboardState.IsKeyDown(Keys.Z) || keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.W) )
            {
                if (player.CanShoot(time))
                {
                    player.HasShot(time);
                    MusicBox.Shoot();
                    Shoot();
                }
            }
            if (keyboardState.IsKeyDown(Keys.M) )
            {
                MusicBox.ToggleSong();
            }
        }

        private void UpdateBullet(GameTime time)
        {
            if (!bullet.Visible)
                return;

            SpriteUtil.MoveSprite(bullet, Direction.North);

            foreach (var enemy in enemies)
            {
                bool hit = bullet.Rectangle.Intersects(enemy.Rectangle);

                if (hit)
                {
                    KillEnemy(enemy);
                    bullet.Visible = false;
                    player.ResetShot();
                    break;
                }
            }
        }

        private void KillEnemy(Enemy enemy)
        {
            enemy.Die();
            MusicBox.Killed();
            player.IncreaseScore(enemyPoints);
            EnemyKilledCount++;
            if (EnemyKilledCount > EnemyKilledCountTillNextLevel)
            {
                World.Level++;
                EnemyKilledCountTillNextLevel = EnemiesKilledToNextLevel(World.Level);
                MusicBox.LevelUp();
            }
        }

        private void EnemyPlot(GameTime time)
        {
            MoveEnemies(time);
            SpawnEnemies(time);
            EnemyShoot(time);
        }

        private void MoveEnemies(GameTime time)
        {
            foreach(var enemy in enemies) 
            {
                enemy.Move();

                if (!player.IsAlive)
                    continue;

                if (enemy.Rectangle.Y > ScreenHeight)
                {
                    EscapeCount++;
                    MusicBox.Escape();
                }

                bool playerHit = player.Rectangle.Intersects(enemy.Rectangle);
                if (playerHit)
                {
                    KillPlayer();
                }
            }
        }

        private void EnemyShoot(GameTime time)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.WillShoot(time))
                {
                    enemy.HasShot(time);
                    var bubble = new Sprite(bullet.Image);
                    SpriteUtil.MoveSprite(bubble, enemy.Rectangle.X, enemy.Rectangle.Y);
                    bubble.Visible = true;
                    enemyBullets.Add(bubble);
                    MusicBox.EnemyShoot();
                }
            }
        }

        private bool lefter = true;
        private void SpawnEnemies(GameTime time)
        {
            if (enemySpawnTimer.Ready(time) || EscapeCount > 5)
            {
                var enemy = new Enemy(lefter, random);
                lefter = !lefter;
                SpriteUtil.MoveSprite(enemy, CrabBattleGame.screenWidth / 2 - enemy.Rectangle.Center.X, enemy.Rectangle.Height + 20);
                enemies.Add(enemy);
                MusicBox.EnemySpawn();
            }
        }

        private void UpdateEnemyBullets(GameTime time)
        {
            foreach (var enemyBullet in enemyBullets)
            {
                if (!enemyBullet.Visible)
                    continue;

                SpriteUtil.MoveSprite(enemyBullet, Direction.South);

                bool hitBullet = bullet.Visible && enemyBullet.Rectangle.Intersects(bullet.Rectangle);
                if (hitBullet)
                {
                    enemyBullet.Visible = false;
                    bullet.Visible = false;
                    player.ResetShot();
                    continue;
                }

                bool hit = enemyBullet.Rectangle.Intersects(player.Rectangle);
                if (hit)
                {
                    KillPlayer();
                    enemyBullet.Visible = false;
                }
            }

            enemyBullets = enemyBullets.Where(a => a.Visible && a.Rectangle.Y < ScreenHeight).ToList();
        }

        private void KillPlayer()
        {
            if (player.IsAlive)
            {
                MusicBox.Killed();
                player.Die();
            }
        }

        private void RevivePlayer()
        {
            player.IsAlive = true;
            player.Score = 0;
            EscapeCount = 0;
            SpriteUtil.MoveSprite(player, CrabBattleGame.screenWidth / 2, player.Rectangle.Y);
            enemyBullets.Clear();
            enemies.Clear();
            MusicBox.Respawn();
            EnemyKilledCount = 0;
            World.Level = 1;
            EnemyKilledCountTillNextLevel = EnemiesKilledToNextLevel(World.Level);
        }

        private void Shoot()
        {
            bullet.Visible = true;
            SpriteUtil.MoveSprite(bullet, player.Rectangle.X, player.Rectangle.Y);
        }

        private int EnemiesKilledToNextLevel(int currentLevel)
        {
            return 20 + (currentLevel - 1) * 10;
        }
    }
}
