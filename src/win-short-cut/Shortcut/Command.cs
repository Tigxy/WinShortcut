using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_short_cut.ShortcutData {
    public class Command {
        public string ExecutionString { get; set; }
        public string Description { get; set; } = "";
        public bool PrintCommand { get; set; } = true;
        public bool PrintDescription { get; set; } = true;
    }
}
