using MIDIReader.Configuration;

namespace MIDIReader.Managers
{
    public interface IAudioManager
    {
        void SetAudio(int audioCode);
    }

    public class AudioManager : IAudioManager
    {
        private readonly IOBSManager _obs;
        private readonly Settings _settings;

        public AudioManager(IOBSManager obs, Settings settings)
        {
            _obs = obs;
            _settings = settings;
        }

        public void SetAudio(int audioCode)
        {
            var audio = _settings.Obs.Audio;

            var audioSources = audio.Sources;
            var volumeLevels = audio.VolumeLevels;
            var audioSettings = audio.Settings[audioCode];

            for (var i = 0; i < audioSettings.Volumes.Length; i++)
            {
                var volumeLevelIndex = audioSettings.Volumes[i];
                _obs.SetInputVolume(audioSources[i], volumeLevels[volumeLevelIndex]);
            }
        }
    }
}