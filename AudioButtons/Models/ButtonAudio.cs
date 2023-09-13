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
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Color { get; set; } = "(0,0,0)";

        public ButtonAudio()
        {
            Id = Guid.NewGuid();
        }

    }
}
