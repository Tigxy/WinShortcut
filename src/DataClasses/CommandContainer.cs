using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace win_short_cut.DataClasses {
    [Serializable]
    public class CommandContainer
    {
        [XmlAttribute]
        public bool KeepOpenOnceDone { get; set; }

        [XmlAttribute]
        public bool ExecuteMinimized { get; set; } = false;

        [XmlAttribute]
        public bool ConcatenateCommands { get; set; }

        [XmlAttribute]
        public string Description { get; set; } = "";

        [XmlAttribute]
        public bool ShowDescriptionFields { get; set; } = false;

        [XmlElement(nameof(Command))]
        public ObservableCollection<Command> Commands { get; set; } = new();

        public bool IsDefault() => EqualsPublic(new());

        public bool EqualsPublic(CommandContainer container) {
            bool propertiesMatch = this.KeepOpenOnceDone.Equals(container.KeepOpenOnceDone)
                && this.ConcatenateCommands.Equals(container.ConcatenateCommands)
                && this.Description.Equals(container.Description)
                && this.ShowDescriptionFields.Equals(container.ShowDescriptionFields);

            // early exit for comparison
            if (!propertiesMatch || this.Commands.Count != container.Commands.Count)
                return false;

            if (this.Commands.Count == container.Commands.Count) {
                for (int i = 0; i < this.Commands.Count; i++) {
                    if (!this.Commands[i].EqualsPublic(container.Commands[i]))
                        return false;
                }
            }

            return true;
        }
    }
}
