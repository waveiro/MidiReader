using System;
using Melanchall.DryWetMidi.Multimedia;
using MIDIReader.Configuration;

namespace MIDIReader.Managers
{
    public interface IMIDIManager: IDisposable
    {
        InputDevice InputDevice { get; }
        void StartListening();
    }
    
    public class MIDIManager: IMIDIManager
    {
        public InputDevice InputDevice { get; }

        public MIDIManager(Settings settings)
        {
            InputDevice = InputDevice.GetByName(settings.Midi.PortName);
        }

        public void StartListening()
        {
            InputDevice.StartEventsListening();
        }

        public void Dispose()
        {
            (InputDevice as IDisposable).Dispose();
        }
    }
}