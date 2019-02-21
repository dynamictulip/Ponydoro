using System.Media;

namespace Pomo_Shiny
{
    internal interface ISoundProvider
    {
        void MakeSoundAsync();
    }

    internal class SoundProvider : ISoundProvider
    {
        public void MakeSoundAsync()
        {
            //get file
            var path = GetRandomSoundFile();

            //play file
            var thing = new SoundPlayer(path);
            thing.Play();
        }

        private static string GetRandomSoundFile()
        {


            var path = @"C:\Meeple\Pomo-Shiny\Pomo-Shiny\Media\07017109.wav";
            return path;
        }
    }
}