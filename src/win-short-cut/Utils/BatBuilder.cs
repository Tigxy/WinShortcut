using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_short_cut.Utils
{
    internal class BatBuilder
    {
        private bool isCommandOutputOn = true;
        private StringBuilder StringBuilder = new();

        public BatBuilder TurnOffCommandOutput()
        {
            // prevent adding unnecessary commands
            if (this.isCommandOutputOn)
            {
                StringBuilder.AppendLine("@echo off");
                isCommandOutputOn = true;
            }
                
            return this;
        }

        public BatBuilder TurnOnCommandOutput()
        {
            // prevent adding unnecessary commands
            if (!this.isCommandOutputOn)
            {
                StringBuilder.AppendLine("@echo on");
                isCommandOutputOn = false;
            }

            return this;
        }

        public BatBuilder Comment(string comment)
        {
            // ensure that all lines of a comment are commented out
            string[] lines = comment.Split('\n');
            lines.ForEach(x => StringBuilder.AppendLine(":: " + x));
            return this;
        }

        public BatBuilder Echo(string text)
        {
            // prevent echo errors
            string s = text.Replace('"', '\'');
            StringBuilder.AppendLine($"@echo {s}");
            return this;
        }

        public BatBuilder Execute(string command) => Execute(command, false);

        public BatBuilder Execute(string command, bool silent)
        {
            StringBuilder.AppendLine((silent ? "@" : "") + command);
            return this;
        }

        public BatBuilder Clear()
        {
            StringBuilder.Clear();
            isCommandOutputOn = true;
            return this;
        }

        public void ToFile(string path)
        {
            using (var sw = new System.IO.StreamWriter(path))
            {
                sw.Write(this.ToString());
            }
        }

        public override string ToString() => StringBuilder.ToString();
    }
}
