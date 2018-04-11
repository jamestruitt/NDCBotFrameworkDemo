using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.LUIS;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using v4ReferenceApp.Context;
using v4ReferenceApp.Models;
using v4ReferenceApp.Responses;

namespace v4ReferenceApp.Subjectable
{
    public class GrannyBotSubject : ISubject
    {
        public string SubjectName { get; set; }

        public ISubject ParentSubject { get; set; }

        public GrannyBotSubject()
        {

        }

        public GrannyBotSubject(ISubject parentSubject)
        {
            ParentSubject = parentSubject;
        }

        public async Task<bool> StartSubject(V4ReferenceContext context)
        {
            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:

                    await context.SendActivity($"Welcome to Granny Bot Central Control");
                    break;

                default:
                    break;
            }
            return true;
        }

        public async Task<bool> ContinueSubject(V4ReferenceContext context)
        {

            var conversation = ConversationState<ConversationData>.Get(context);

            if (context.Activity.Type == ActivityTypes.Message)
            {

                var luisResult =
                    context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);

                if (luisResult != null)
                {
                    (string key, double score) topItem = luisResult.GetTopScoringIntent();
                    
                    switch (topItem.key)
                    {
                        case "GetActivities":
                            var activityId = luisResult.Entities.GetValue("ActivityID")[0].Value<string>();
                            await context.SendActivity($" Found Entity Name: ActivityID : {activityId}");
                            await context.SendActivities(BotResponses.GetSingle(activityId));
                            break;
                        case "ListActivities":
                            await context.SendActivities(BotResponses.GetMultiple());
                            break;
                        case "SendAlert":
                            var xactivityId = luisResult.Entities.GetValue("ActivityID")[0].Value<string>();
                            await context.SendActivity($" Alert for Activity: {xactivityId} has been sent");
                            break;
                        case "CancelAlert":
                            var yactivityId = luisResult.Entities.GetValue("ActivityID")[0].Value<string>();
                            await context.SendActivity($"Activity : {yactivityId} has been cancelled");
                            break;
                        case "Help":
                            // show help
                            await GuessingGamesSubjectResponses.ReplyWithHelp(context);
                            break;

                        case "MainMenu":
                            // show Main Menu
                            conversation.CurrentSubject = conversation.MainMenuSubject;
                            await conversation.MainMenuSubject.ContinueSubject(context);
                            break;

                        case "Quit":
                            // show Main Menu
                            conversation.CurrentSubject = ParentSubject;
                            await ParentSubject.ContinueSubject(context);
                            break;
                        default:
                            // show our confusion
                            await GuessingGamesSubjectResponses.ReplyWithConfused(context);
                            break;
                    }

                }
            }

            return true;
        }
    }
}
