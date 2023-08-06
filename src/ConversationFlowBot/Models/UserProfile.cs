using System.Collections.Generic;

namespace ConversationFlowBot.Models;

/// <summary>Contains information about a user.</summary>
public class UserProfile
{
    public string Name { get; set; }

    public int Age { get; set; }

    // The list of companies the user wants to review.
    public List<string> CompaniesToReview { get; set; } = new List<string>();
}
