using Spectre.Console;

namespace CodingTracker.BBualdo;

public class ConsoleEngine
{
  public static string GetSelection(string title, string[] choices)
  {
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine($"--------[bold green]{title}[/]--------");

    string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                    .Title("What would you like to do?")
                                    .HighlightStyle(new Style().Foreground(Color.Green))
                                    .AddChoices(choices)
                                    );

    return choice;
  }
}
