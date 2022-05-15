using System;
using MIDIReader.Configuration;
using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;

namespace MIDIReader.Managers
{
    public interface IOBSManager
    {
        void Connect();
        void Disconnect();
        void SetCurrentScene(string sceneName);
        void SetInputVolume(string source, double volume);
    }

    public class OBSManager : IOBSManager
    {
        private readonly OBSWebsocket _obsWebsocket;
        private readonly Settings _settings;

        public OBSManager(Settings settings)
        {
            _obsWebsocket = new OBSWebsocket();
            _settings = settings;
        }

        public void Connect()
        {
            _obsWebsocket.Connect(_settings.Obs.Url, _settings.Obs.Password);
        }

        public void Disconnect()
        {
            _obsWebsocket.Disconnect();
        }

        public void SetCurrentScene(string sceneName)
        {
            _obsWebsocket.SetCurrentScene(sceneName);
        }

        public void SetInputVolume(string source, double volume)
        {
            var requestFields = new JObject
            {
                { "source", source },
                { "volume", volume },
                { "useDecibel", true }
            };
            
            _obsWebsocket.SendRequest("SetVolume", requestFields);
        }
    }
}