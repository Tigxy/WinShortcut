using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_short_cut.ShortcutData {
    public class CommandContainer {
        public string Description { get; set; } = "";
        public Command[] Commands { get; set; }
        public bool KeepOpenOnceDone { get; set; }
    }
}
