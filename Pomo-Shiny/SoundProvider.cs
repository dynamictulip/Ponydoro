using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Media;
using Pomo_Shiny.Properties;

namespace Pomo_Shiny
{
    public interface ISoundProvider
    {
        void MakeSound();
    }

    [ExcludeFromCodeCoverage]
    internal class SoundProvider : ISoundProvider
    {
        private readonly List<SoundPlayer> _players;

        public SoundProvider()
        {
            var resourceStreams = new List<UnmanagedMemoryStream>
            {
                Resources._07017109,
                Resources._07042154,
                Resources._07042293,
                Resources._07043261,
                Resources._07043397,
                Resources._07050147,
                Resources._07070150
            };

            _players = resourceStreams.Select(r =>
            {
                var soundPlayer = new SoundPlayer(r);
                soundPlayer.LoadAsync();
                return soundPlayer;
            }).ToList();
        }

        public void MakeSound()
        {
            GetRandomSoundPlayer().Play();
        }

        private SoundPlayer GetRandomSoundPlayer()
        {
            var rand = new Random().Next(0, _players.Count - 1);
            return _players[rand];
        }
    }
}