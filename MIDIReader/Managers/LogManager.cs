using System;
using MIDIReader.Configuration;

namespace MIDIReader.Managers
{
    public interface ILogManager
    {
        void Info(int channelCode, int noteCode);
        void Error(int channelCode, int noteCode);
    }
    
    public class LogManager : ILogManager
    {
        private readonly Settings _settings;
        
        public LogManager(Settings settings)
        {
            _settings = settings;
        }
        
        public void Info(int channelCode, int noteCode)
        {
            var channels = _settings.Midi.Channels;
            var channel = channels[channelCode];
            var note = "";
            
            switch (channel)
            {
                case "Scene":
                    note = _settings.Obs.Scenes[noteCode];
                    break;
                case "Audio":
                    note = _settings.Obs.Audio.Settings[noteCode].Name;
                    break;
            }
            
            Console.WriteLine($"Channel: {channel} Note: {note}");
        }
        public void Error(int channelCode, int noteCode)
        {
            Console.Clear();
            Console.WriteLine("********************");
            Info(channelCode, noteCode);
            Console.WriteLine("********************");
        }

        
        
    }
}