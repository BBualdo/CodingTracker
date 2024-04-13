using CodingTracker.Database.Models;
using Spectre.Console;

namespace CodingTracker.Database.Helpers;

public class ConsoleEngine
{
  public static string GetSelection(string header, string title, string[] choices)
  {
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine($"--------[bold green]{header}[/]--------");

    string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                    .Title(title)
                                    .HighlightStyle(new Style().Foreground(Color.Green))
                                    .AddChoices(choices)
                                    );

    return choice;
  }

  public static void GetCodingSessionsTable(List<CodingSession> sessions)
  {
    if (sessions.Count == 0)
    {
      AnsiConsole.WriteLine("Sessions list is empty. Create one first. Press any key to return to Main Menu.");
      Console.ReadKey();
      return;
    }

    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Start Date"));
    table.AddColumn(new TableColumn("End Date"));
    table.AddColumn(new TableColumn("Duration"));

    foreach (CodingSession session in sessions)
    {
      table.AddRow(session.Session_Id.ToString(), session.Start_Date.ToString(), session.End_Date.ToString(), session.Duration.ToString());
    }

    AnsiConsole.Write(table);
  }
}
