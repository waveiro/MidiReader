using System;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using MIDIReader.Configuration;
using MIDIReader.Managers;
using Newtonsoft.Json;

namespace MIDIReader
{
    internal class Program
    {
        private static IMIDIManager _midiManager;
        private static IOBSManager _obsManager;
        private static IAudioManager _audioManager;
        private static IScreenManager _screenManager;
        private static ILogManager _logManager;

        private static Settings _settings;


        private static void Main(string[] args)
        {
            try
            {
                ReadSettings();
                InitializeOBS();
                InitializeMIDI();
                InitializeManagers();

                Console.WriteLine("Input device is listening for events. Press any key to exit...");
                Console.ReadKey();
            }
            finally
            {
                _obsManager.Disconnect();
                _midiManager.Dispose();
            }
        }

        private static void ReadSettings()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
        }

        private static void InitializeOBS()
        {
            _obsManager = new OBSManager(_settings);
            _obsManager.Connect();
        }

        private static void InitializeMIDI()
        {
            _midiManager = new MIDIManager(_settings);
            _midiManager.InputDevice.EventReceived += OnEventReceived;
            _midiManager.StartListening();
        }

        private static void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            var noteOnEvent = (NoteOnEvent)e.Event;

            var channelCode = Convert.ToInt32(noteOnEvent.Channel);
            var noteCode = Convert.ToInt32(noteOnEvent.NoteNumber);

            var channel = _settings.Midi.Channels[channelCode];

            if (_settings.GeneralSettings.LogEnabled)
            {
                _logManager.Info(channelCode, noteCode);
            }

            switch (channel)
            {
                case "Scene":
                    _screenManager.SetScreen(noteCode);
                    break;
                case "Audio":
                    _audioManager.SetAudio(noteCode);
                    break;
                default:
                    _logManager.Error(channelCode, noteCode);
                    break;
            }
        }

        private static void InitializeManagers()
        {
            _audioManager = new AudioManager(_obsManager, _settings);
            _screenManager = new ScreenManager(_obsManager, _settings);
            _logManager = new LogManager(_settings);
        }
    }
}