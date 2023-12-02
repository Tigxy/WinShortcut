using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using win_short_cut.Utils;

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

        public Shortcut? DeepCopy() => this.DeepCopy(true);
        public Shortcut? DeepCopy(bool copyId) {
            if (this.DeepClone() is Shortcut sc) {
                if (copyId)
                    sc.Id = this.Id;
                return sc;
            }
            return default;
        }
    }
}
