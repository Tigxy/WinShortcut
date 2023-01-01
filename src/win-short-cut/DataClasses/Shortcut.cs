using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    [Serializable]
    public class Shortcut {
        [XmlIgnore]
        internal Guid Id = System.Guid.NewGuid();

        [XmlAttribute]
        public string Name { get; set; } = "";

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlElement(nameof(CommandContainer))]
        public ObservableCollection<CommandContainer> Containers { get; set; } = new();
    }
}
