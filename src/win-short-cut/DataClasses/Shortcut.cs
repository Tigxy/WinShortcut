using System;
using System.Collections.ObjectModel;
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
