using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CrabBattle.GameLogic
{
    class MusicBox
    {
        public static Song MainSong;
        public static SoundEffectInstance ShootSound;
        public static SoundEffectInstance EnemyShootSound;
        public static SoundEffectInstance KilledSound;
        public static SoundEffectInstance RespawnSound;
        public static SoundEffectInstance EnemySpawnSound;
        public static SoundEffectInstance EscapeSound;
        public static SoundEffectInstance LevelUpSound;

        private static bool SongPlaying = true;

        public static void ToggleSong()
        {
            if (SongPlaying)
            {
                EndSong();
            }
            else
            {
                MediaPlayer.Resume();
            }

            SongPlaying = !SongPlaying;
        }

        public static void StartSong()
        {
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MainSong);
        }

        public static void EndSong()
        {
            MediaPlayer.Stop();
        }

        public static void Shoot()
        {
            ShootSound.Play();
        }

        public static void EnemyShoot()
        {
            EnemyShootSound.Play();
        }

        public static void Killed()
        {
            KilledSound.Play();
        }

        public static void Respawn()
        {
            RespawnSound.Play();
        }

        public static void EnemySpawn()
        {
            EnemySpawnSound.Play();
        }

        public static void Escape()
        {
            EscapeSound.Play();
        }

        public static void LevelUp()
        {
            LevelUpSound.Play();
        }
    }
}
