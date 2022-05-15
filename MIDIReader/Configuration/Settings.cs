using Newtonsoft.Json;

namespace MIDIReader.Configuration
{
    public class Settings
    {
        [JsonProperty(PropertyName = "obs")] 
        public ObsSettingsJson Obs { get; set; }

        [JsonProperty(PropertyName = "midi")] 
        public MidiSettingsJson Midi { get; set; }
        
        [JsonProperty(PropertyName = "settings")] 
        public GeneralSettingsJson GeneralSettings { get; set; }
    }

    public class GeneralSettingsJson
    {
        [JsonProperty(PropertyName = "logEnabled")]
        public bool LogEnabled { get; set; }
    }

    public class ObsSettingsJson
    {
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "url")] 
        public string Url { get; set; }

        [JsonProperty(PropertyName = "scenes")]
        public string[] Scenes { get; set; }

        [JsonProperty(PropertyName = "audio")] 
        public AudioJson Audio { get; set; }
    }

    public class AudioJson
    {
        [JsonProperty(PropertyName = "sources")]
        public string[] Sources { get; set; }

        [JsonProperty(PropertyName = "settings")]
        public AudioSettings[] Settings { get; set; }
        
        [JsonProperty(PropertyName = "volumeLevels")]
        public double[] VolumeLevels { get; set; }
    }

    public class AudioSettings
    {
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; set; }

        [JsonProperty(PropertyName = "volumes")]
        public int[] Volumes { get; set; }
    }


    public class MidiSettingsJson
    {
        [JsonProperty(PropertyName = "portName")]
        public string PortName { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public string[] Channels { get; set; }
    }
}