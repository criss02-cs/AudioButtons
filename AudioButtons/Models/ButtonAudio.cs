using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;

namespace AudioButtons.Models
{
    public class ButtonAudio
    {
        private Audio _audio = new();
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerializedAudio { get; set; } = string.Empty;

        public ButtonAudio()
        {
            Id = Guid.NewGuid();
        }

        [Ignore]
        public Audio Audio
        {
            get => _audio;
            set
            {
                if (_audio == value) return;
                _audio = value;
                SerializedAudio = JsonConvert.SerializeObject(value);
            }
        }
    }
}
