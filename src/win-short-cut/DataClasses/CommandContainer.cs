using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    [Serializable]
    public class CommandContainer
    {
        [XmlAttribute]
        public bool KeepOpenOnceDone { get; set; }

        [XmlAttribute]
        public bool ConcatenateCommands { get; set; }

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlAttribute]
        public bool ShowDescriptionFields { get; set; } = false;

        [XmlElement(nameof(Command))]
        public ObservableCollection<Command> Commands { get; set; } = new();
    }
}
