﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.LUIS;
using Microsoft.Cognitive.LUIS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using v4ReferenceApp.Bots;
using v4ReferenceApp.Models;

namespace v4ReferenceApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            services.AddBot<MainBot>(options =>
            {
                options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);
             
                var middleware = options.Middleware;

                // Add middleware to send an appropriate message to the user if an exception occurs
                middleware.Add(new CatchExceptionMiddleware<Exception>(async (context, exception) =>
                {
                    await context.SendActivity($"Sorry, it looks like something went wrong! {exception.Message}");
                }));

                middleware.Add(new ConversationState<ConversationData>(new MemoryStorage()));

                middleware.Add(new ShowTypingMiddleware());

                var luisModelId = "31d73ce3-395f-4576-805d-eed1638d06d7";
                var luisSubscriptionKey = "646f989672a54f33b838d3ecdefc8b6e";
                var luisUri = new Uri("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/");

                var luisModel = new LuisModel(luisModelId, luisSubscriptionKey, luisUri);

                // If you want to get all intents scorings, add verbose in luisOptions
                var luisOptions = new LuisRequest { Verbose = true };

                middleware.Add(new LuisRecognizerMiddleware(luisModel, luisOptions: luisOptions));

                //var defaultConnection =
                //    Configuration["ConnectionStrings:DefaultConnection"];

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseBotFramework();
     
        }
    }
}