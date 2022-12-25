using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    [XmlType(TypeName = "Shortcuts")]
    public class Shortcuts : List<Shortcut> {
        public Shortcuts() : base() { }
        public Shortcuts(int capacity) : base(capacity) { }
        public Shortcuts(IEnumerable<Shortcut> collection) : base(collection) { }
    }
}
