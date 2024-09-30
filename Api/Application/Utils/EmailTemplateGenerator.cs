using Api.Domain;

namespace Api.Application;

public static class EmailTemplateGenerator
{
    public static EmailContent GetBudgetIncreaseEmail(string userEmail, decimal percent)
    {
        var emailSubject = $"You managed to exceed {percent}% of your budget";
        var emailBody = $"Congratulations! 🥳 You have already increased more than {percent}% of your budget. 💰";
        var email = new EmailContent(userEmail, emailSubject, emailBody);

        return email;
    }

    public static EmailContent GetBudgetDecreaseEmail(string userEmail, decimal percent)
    {
        var emailSubject = $"You lost {percent}% of your budget";
        var emailBody = $"Oh no! ☹️ You've already lost {percent}% of your budget, now it's time to reduce expenses and increase income. 💸";
        var email = new EmailContent(userEmail, emailSubject, emailBody);

        return email;
    }
}