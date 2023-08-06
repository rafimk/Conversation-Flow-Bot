using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ConversationFlowBot.Dialogs;

public class OrderFoodDialog : ComponentDialog
{
    // Dialog IDs
    private const string OrderFoodDialogId = "orderFoodDialog";

    // Prompt IDs
    private const string FoodChoicePrompt = "foodChoicePrompt";
    private const string QuantityPrompt = "quantityPrompt";
    private const string ConfirmOrderPrompt = "confirmOrderPrompt";

    public OrderFoodDialog()
        : base(OrderFoodDialogId)
    {
        // Add prompts to the dialog
        AddDialog(new TextPrompt(FoodChoicePrompt));
        AddDialog(new NumberPrompt<int>(QuantityPrompt));
        AddDialog(new ConfirmPrompt(ConfirmOrderPrompt));

        // Add waterfall steps
        WaterfallStep[] waterfallSteps = new WaterfallStep[]
        {
            ChooseFoodStepAsync,
            ChooseQuantityStepAsync,
            ConfirmOrderStepAsync,
            FinalStepAsync
        };
        AddDialog(new WaterfallDialog(OrderFoodDialogId, waterfallSteps));

        // Set the starting dialog for the component
        InitialDialogId = OrderFoodDialogId;
    }

    private async Task<DialogTurnResult> ChooseFoodStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        // Your logic to get the food menu from a database or API
        List<string> foodMenu = new List<string> { "Pizza", "Burger", "Pasta", "Salad" };

        // Prompt the user to choose food from the menu
        return await stepContext.PromptAsync(FoodChoicePrompt, new PromptOptions
        {
            Prompt = MessageFactory.Text("What would you like to order? We have Pizza, Burger, Pasta, and Salad."),
            Choices = ChoiceFactory.ToChoices(foodMenu)
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> ChooseQuantityStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        // Save the user's food choice in the step context
        stepContext.Values["foodChoice"] = ((FoundChoice)stepContext.Result).Value;

        // Prompt the user to choose the quantity
        return await stepContext.PromptAsync(QuantityPrompt, new PromptOptions
        {
            Prompt = MessageFactory.Text("How many would you like to order?")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> ConfirmOrderStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        // Save the user's quantity choice in the step context
        stepContext.Values["quantity"] = (int)stepContext.Result;

        // Prompt the user to confirm the order
        return await stepContext.PromptAsync(ConfirmOrderPrompt, new PromptOptions
        {
            Prompt = MessageFactory.Text($"You ordered {stepContext.Values["quantity"]} {stepContext.Values["foodChoice"]}(s). Is this correct?")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        // Handle the user's confirmation
        bool isConfirmed = (bool)stepContext.Result;

        if (isConfirmed)
        {
            // Your logic to process the order and send a confirmation message
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thank you for your order! It will be delivered shortly."), cancellationToken);
        }
        else
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Order canceled. If you need any assistance, feel free to ask."), cancellationToken);
        }

        // End the dialog
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }
}