using MIDIReader.Configuration;

namespace MIDIReader.Managers
{
    public interface IScreenManager
    {
        void SetScreen(int sceneCode);
    }

    public class ScreenManager : IScreenManager
    {
        private readonly IOBSManager _obs;
        private readonly Settings _settings;

        public ScreenManager(IOBSManager obs, Settings settings)
        {
            _obs = obs;
            _settings = settings;
        }

        public void SetScreen(int sceneCode)
        {
            var command = _settings.Obs.Scenes[sceneCode];
            _obs.SetCurrentScene(command);
        }
    }
}