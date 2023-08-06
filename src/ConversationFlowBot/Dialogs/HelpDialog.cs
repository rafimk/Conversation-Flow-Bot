using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System.Threading.Tasks;
using System.Threading;

namespace ConversationFlowBot.Dialogs;

public class HelpDialog : ComponentDialog
{
    public HelpDialog()
        : base(nameof(HelpDialog))
    {
        // Set the starting dialog for the component
        InitialDialogId = nameof(HelpDialog);

        // Add a single dialog step to display the help message
        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
        {
            DisplayHelpStepAsync,
            FinalStepAsync
        }));
    }

    private async Task<DialogTurnResult> DisplayHelpStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        // Provide the help message to the user
        await stepContext.Context.SendActivityAsync(MessageFactory.Text("I'm here to help you with your order. Please type 'order food' to start the ordering process or 'help' if you need assistance."), cancellationToken);

        // End the dialog
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

    private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        // End the dialog
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }
}