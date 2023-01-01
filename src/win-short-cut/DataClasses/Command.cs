using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    [Serializable]
    public class Command {
        [XmlIgnore]
        internal Guid Id = System.Guid.NewGuid();

        [XmlAttribute]
        public bool PrintCommand { get; set; } = false;

        [XmlAttribute]
        public bool PrintDescription { get; set; } = false;

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlText]
        public string ExecutionString { get; set; } = "";
    }
}
