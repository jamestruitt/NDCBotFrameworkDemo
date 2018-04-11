using v4ReferenceApp.Subjectable;

namespace v4ReferenceApp.Models
{
    public class ConversationData
    {
        public bool IsGreeted { get; set; }

        public ISubject CurrentSubject { get; set; }

        public ISubject MainMenuSubject { get; set; }

        public int SecretNumber { get; set; }
    }
}
