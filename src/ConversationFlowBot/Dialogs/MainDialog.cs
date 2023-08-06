// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.18.1
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace ConversationFlowBot.Dialogs
{
    public class MainDialog : ActivityHandler
    {
        private readonly DialogSet _dialogs;

        public MainDialog()
        {
            // Create a new DialogSet instance
            _dialogs = new DialogSet();

            // Add the Top-Level Dialog to the DialogSet
            _dialogs.Add(new TopLevelDialog());
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Create a DialogContext to process the incoming activity
            var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);

            // Continue the active dialog (if any)
            var dialogResult = await dialogContext.ContinueDialogAsync(cancellationToken);

            // If no active dialog, start the Top-Level Dialog
            if (!dialogContext.Context.Responded)
            {
                await dialogContext.BeginDialogAsync(nameof(TopLevelDialog), cancellationToken: cancellationToken);
            }
        }
    }
}
