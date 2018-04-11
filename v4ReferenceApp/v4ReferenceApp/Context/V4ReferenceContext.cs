using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using v4ReferenceApp.Models;

namespace v4ReferenceApp.Context
{
    public class V4ReferenceContext: TurnContextWrapper
    {
        public V4ReferenceContext(ITurnContext context) : base(context)
        {
        }

        public ConversationData ConversationState
        {
            get
            {
                return ConversationState<ConversationData>.Get(this);
            }
        }
    }
}
