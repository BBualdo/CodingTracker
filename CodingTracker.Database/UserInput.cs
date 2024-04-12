using CodingTracker.Database.Helpers;
using Spectre.Console;

namespace CodingTracker.Database;

public class UserInput
{
  public static string GetStartDate()
  {
    string startDate = AnsiConsole.Ask<string>("Enter date and time when you started your coding session. ([green]Example: 11-04-24 10:20[/]) ");

    while (!DateTimeValidator.IsValid(startDate))
    {
      startDate = AnsiConsole.Ask<string>("Try again: ");
    }

    return startDate;
  }

  public static string GetEndDate(string startDate)
  {
    string endDate = AnsiConsole.Ask<string>("Enter date and time when you finished your coding session. ([green]Example: 11-04-24 14:20[/]) ");

    while (!DateTimeValidator.IsValid(endDate))
    {
      endDate = AnsiConsole.Ask<string>("Try again: ");
    }

    while (!DateTimeValidator.AreValid(startDate, endDate))
    {
      endDate = AnsiConsole.Ask<string>($"Try again: ");
    };

    return endDate;
  }
}
