using CrabBattle.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrabBattle.GameLogic
{
    public class CrabBattleGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int screenHeight;
        public static int screenWidth;

        World world;

        public CrabBattleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            world = new World();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadGraphicContent();
            LoadMusicContent();
        }

        private void LoadGraphicContent()
        {
            screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;

            world.ScreenHeight = screenHeight;

            var caveTexture = Content.Load<Texture2D>("Images/Cave");
            var cave = new Sprite(caveTexture);
            SpriteUtil.MoveSprite(cave, -100, -90);
            world.cave = cave;

            var waterTexture = Content.Load<Texture2D>("Images/Water");
            var water = new Sprite(waterTexture);
            SpriteUtil.MoveSprite(water, 0, screenHeight - water.Rectangle.Height);
            world.water = water;

            Crab.ReadyImage = Content.Load<Texture2D>("Images/8BitCrabFoam");
            Crab.StandbyImage = Content.Load<Texture2D>("Images/8BitCrab");
            Crab.DeadImage = Content.Load<Texture2D>("Images/8BitCrabDead");

            var mPlayer = new Player();
            world.player = mPlayer;

            SpriteUtil.MoveSprite(mPlayer, screenWidth / 2 - mPlayer.Rectangle.Center.X, screenHeight - mPlayer.Rectangle.Height - 20);

            var textureBullet = Content.Load<Texture2D>("Images/Bubble");
            var bullet = new Sprite(textureBullet);
            bullet.Visible = false;
            world.bullet = bullet;

            var font = Content.Load<SpriteFont>("Font/SpriteFont");
            world.font = font;
            var fontPos = new Vector2(60, 20);
            world.scoreFontPos = fontPos;

            fontPos = new Vector2(70, 50);
            world.escapeFontPos = fontPos;

            fontPos = new Vector2(70, 80);
            world.thirdFontPos = fontPos;

            fontPos = new Vector2(70, 110);
            world.fourthFontPos = fontPos;
        }

        private void LoadMusicContent()
        {
            var song = Content.Load<Song>("Sound\\Music\\DST-5thStreet");
            MusicBox.MainSong = song;

            var soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_01");
            var soundEngineInstance = soundEngine.CreateInstance();
            MusicBox.ShootSound = soundEngineInstance;

            soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_01");
            soundEngineInstance = soundEngine.CreateInstance();
            soundEngineInstance.Volume = 0.4f;
            MusicBox.EnemyShootSound = soundEngineInstance;

            soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_11");
            soundEngineInstance = soundEngine.CreateInstance();
            soundEngineInstance.Volume = 0.6f;
            MusicBox.KilledSound = soundEngineInstance;

            soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_04");
            soundEngineInstance = soundEngine.CreateInstance();
            MusicBox.RespawnSound = soundEngineInstance;

            soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_09");
            soundEngineInstance = soundEngine.CreateInstance();
            soundEngineInstance.Volume = 0.4f;
            MusicBox.EnemySpawnSound = soundEngineInstance;

            soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_07");
            soundEngineInstance = soundEngine.CreateInstance();
            MusicBox.EscapeSound = soundEngineInstance;

            soundEngine = Content.Load<SoundEffect>("Sound\\Effects\\jump_06");
            soundEngineInstance = soundEngine.CreateInstance();
            MusicBox.LevelUpSound = soundEngineInstance;

            MusicBox.StartSong();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (keyboardState.IsKeyDown(Keys.RightAlt) && keyboardState.IsKeyDown(Keys.Enter))
            {
                graphics.ToggleFullScreen();
            }

            world.UpdateWorld(gameTime, keyboardState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SandyBrown);

            spriteBatch.Begin();

            world.DrawWorld(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
