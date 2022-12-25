using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    public class Command
    {
        [XmlAttribute]
        public bool PrintCommand { get; set; } = true;

        [XmlAttribute]
        public bool PrintDescription { get; set; } = true;

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlText]
        public string ExecutionString { get; set; } = "";
    }
}
