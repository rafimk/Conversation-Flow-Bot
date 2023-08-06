using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Schema;

namespace ConversationFlowBot.Dialogs;
public class TopLevelDialog : ComponentDialog
{
    // Dialog IDs
    private const string OrderFoodDialogId = "orderFoodDialog";
    private const string HelpDialogId = "helpDialog";

    public TopLevelDialog()
        : base(nameof(TopLevelDialog))
    {
        // Add child dialogs to the top-level dialog
        AddDialog(new OrderFoodDialog());
        AddDialog(new HelpDialog());

        // Set the starting dialog for the component
        InitialDialogId = nameof(TopLevelDialog);
    }

    protected override async Task<DialogTurnResult> OnBeginDialogAsync(DialogContext innerDc, object options, CancellationToken cancellationToken = default)
    {
        // Override OnBeginDialogAsync to add any additional setup logic before starting the dialog
        return await RunDialogAsync(innerDc, cancellationToken);
    }

    protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default)
    {
        // Override OnContinueDialogAsync to handle any continuation logic during the conversation
        return await RunDialogAsync(innerDc, cancellationToken);
    }

    private async Task<DialogTurnResult> RunDialogAsync(DialogContext innerDc, CancellationToken cancellationToken)
    {
        // Get the current activity from the context
        var activity = innerDc.Context.Activity;

        // Check the user's intent or message type and route the conversation accordingly
        switch (activity.Type)
        {
            case ActivityTypes.Message:
                // If the user's input is "order food", start the OrderFoodDialog
                if (activity.Text.ToLower().Contains("order food"))
                {
                    return await innerDc.BeginDialogAsync(OrderFoodDialogId, cancellationToken: cancellationToken);
                }
                // If the user's input is "help", start the HelpDialog
                else if (activity.Text.ToLower().Contains("help"))
                {
                    return await innerDc.BeginDialogAsync(HelpDialogId, cancellationToken: cancellationToken);
                }
                // If the user's input doesn't match any intent, send a default response
                else
                {
                    await innerDc.Context.SendActivityAsync(MessageFactory.Text("Sorry, I didn't understand that. How can I assist you?"), cancellationToken);
                    return await innerDc.EndDialogAsync(cancellationToken: cancellationToken);
                }

            // ... (other cases)

            default:
                // Handle other activity types (e.g., typing, ping, etc.)
                return await innerDc.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
