using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    public class Shortcut {
        [XmlAttribute]
        public string Name { get; set; } = "";

        //[XmlAttribute]
        //public bool ExecuteSequentially { get; set; } = true;

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlElement(nameof(CommandContainer))]
        public CommandContainer[] Containers { get; set; } = Array.Empty<CommandContainer>();
    }
}
