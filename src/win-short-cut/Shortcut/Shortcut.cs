using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_short_cut.ShortcutData {
    public class Shortcut {
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public CommandContainer[] Containers { get; set; }
    }
}
