﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;

namespace v4ReferenceApp.Responses
{
    public class MainSubjectResponses
    {
        public static async Task ReplyWithGreeting(ITurnContext context)
        {
           
            Activity replyToConversation = MessageFactory.Carousel(new List<Attachment>()) as Activity;
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            List<CardImage> cardImages = new List<CardImage>
            {
                new CardImage(url: "http://4.bp.blogspot.com/_-jGkZ83iwuQ/S9TcnNcdoyI/AAAAAAAAAcw/COgFyJoV3e8/S180/Main+Menu+Icon.jpg")
            };

            List<CardAction> cardButtons = new List<CardAction>();

            CardAction sendAlertButton = new CardAction()
            {
                Value = $"games",
                Type = "imBack",
                Title = "Play a Game"
            };

            CardAction canelAlertButton = new CardAction()
            {
                Value = $"granny",
                Type = "imBack",
                Title = "Granny Bot"
            };

            cardButtons.Add(sendAlertButton);
            cardButtons.Add(canelAlertButton);

            HeroCard plCard = new HeroCard()
            {
                Title = "Please Select an Item",
                Images = cardImages,
                Buttons = cardButtons,
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            var activities = new IActivity[] {MessageFactory.Text("Hello, I'm the V4Reference Bot.") , replyToConversation};

            await context.SendActivities(activities);

        }
        public static async Task ReplyWithHelp(ITurnContext context)
        {
            await context.SendActivity($"I can play games and other simple tasks. ");
        }
        public static async Task ReplyWithResumeTopic(ITurnContext context)
        {
            await context.SendActivity($"Welcome ba");
        }
        public static async Task ReplyWithConfused(ITurnContext context)
        {
            await context.SendActivity($"I am sorry, I didn't understand that.");
        }
    }
}