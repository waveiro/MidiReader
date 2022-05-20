using System;
using System.Collections.Concurrent;
using System.Timers;
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

        private readonly ConcurrentStack<int> _actions;
        private readonly ConcurrentStack<Timer> _timers;

        private const int BACKCAMERACODE = 0;
        private const int FRONTCAMERACODE = 1;

        public ScreenManager(IOBSManager obs, Settings settings)
        {
            _obs = obs;
            _settings = settings;
            _actions = new ConcurrentStack<int>();
            _actions.Push(FRONTCAMERACODE);

            _timers = new ConcurrentStack<Timer>();
        }

        public void SetScreen(int sceneCode)
        {
            RemoveLastTimer();
            var command = _settings.Obs.Scenes[sceneCode];

            if (command != "Music")
            {
                _obs.SetCurrentScene(command);
                return;
            }

            StartMusicScene();
        }

        private void StartMusicScene()
        {
            _actions.TryPeek(out var codeFromActions);
            var command = _settings.Obs.Scenes[codeFromActions];
            _obs.SetCurrentScene(command);

            var timer = CreateTimer();
            timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            RemoveLastTimer();
            _actions.TryPop(out var codeFromActions);
            var nextCode = codeFromActions == FRONTCAMERACODE ? BACKCAMERACODE : FRONTCAMERACODE;
            _actions.Push(nextCode);

            var command = _settings.Obs.Scenes[nextCode];
            _obs.SetCurrentScene(command);

            var timer = CreateTimer();
            timer.Start();
        }

        private void RemoveLastTimer()
        {
            if (_timers.TryPop(out var timerToDispose))
            {
                timerToDispose.Stop();
                timerToDispose.Dispose();
            }
        }

        private Timer CreateTimer()
        {
            var timer = new Timer();
            timer.Interval = TimeSpan.FromSeconds(_settings.GeneralSettings.SecondsToChangeScene).TotalMilliseconds;
            timer.Elapsed += TimerOnElapsed;

            _timers.Push(timer);

            return timer;
        }
    }
}