using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    [Serializable]
    public class Command: INotifyPropertyChanged {
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

        private bool _isDescriptionFocused = false;
        /// <summary>
        /// UI flag to determine whether the description field of this command is focused. 
        /// </summary>
        [XmlIgnore]
        public bool IsDescriptionFocused {
            get => _isDescriptionFocused;
            set {
                _isDescriptionFocused = value;
                OnPropertyChanged(nameof(IsDescriptionFocused));
            }
        }

        private bool _isCommandFocused = false;
        /// <summary>
        /// UI flag to determine whether the command field of this command is focused. 
        /// </summary>
        [XmlIgnore]
        public bool IsCommandFocused {
            get => _isCommandFocused;
            set {
                _isCommandFocused = value;
                OnPropertyChanged(nameof(IsCommandFocused));
            }
        }

        // NonSerialized field required as otherwise this would throw an exception, see https://stackoverflow.com/q/618994
        [field:NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public bool IsDefault() => EqualsPublic(new());

        public bool EqualsPublic(Command command) {
            return this.Description.Equals(command.Description)
                && this.ExecutionString.Equals(command.ExecutionString)
                && this.PrintDescription.Equals(command.PrintDescription)
                && this.PrintCommand.Equals(command.PrintCommand);
        }
    }
}
