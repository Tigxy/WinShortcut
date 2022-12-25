using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    public class CommandContainer
    {
        [XmlAttribute]
        public bool KeepOpenOnceDone { get; set; }

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlElement(nameof(Command))]
        public Command[] Commands { get; set; } = Array.Empty<Command>();
    }
}
