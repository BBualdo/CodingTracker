using CodingTracker.Database;
using CodingTracker.Database.Helpers;
using Spectre.Console;

namespace CodingTracker.BBualdo;

public class AppEngine
{
  public DbContext Db { get; set; }
  public bool IsRunning { get; set; }
  public Stopwatch Stopwatch { get; set; }

  public AppEngine()
  {
    Db = new DbContext();
    IsRunning = true;
    Stopwatch = new Stopwatch();
  }

  public void MainMenu()
  {
    string choice = ConsoleEngine.GetSelection("MAIN MENU", "What would you like to do?", ["Start Coding", "Sessions", "Goals", "Close App"]);

    switch (choice)
    {
      case "Start Coding":
        string[]? dates = StartCoding();

        if (dates == null)
        {
          AnsiConsole.Markup("[red]Something went wrong :([/]");
          break;
        }

        Db.InsertSession(dates[0], dates[1]);
        break;
      case "Sessions":
        SessionsMenu();
        break;
      case "Goals":
        GoalsMenu();
        break;
      case "Close App":
        AnsiConsole.Markup("[green]Goodbye![/]");
        IsRunning = false;
        break;
    }
  }

  public void SessionsMenu()
  {
    string choice = ConsoleEngine.GetSelection("SESSIONS MENU", "What would you like to do?", ["Back", "Get All Sessions", "Insert Session", "Update Session", "Delete Session"]);

    switch (choice)
    {
      case "Back":
        break;
      case "Get All Sessions":
        Db.GetAllSessions();
        break;
      case "Insert Session":
        Db.InsertSession();
        break;
      case "Update Session":
        Db.UpdateSession();
        break;
      case "Delete Session":
        Db.DeleteSession();
        break;
    }
  }

  public void GoalsMenu()
  {
    string choice = ConsoleEngine.GetSelection("GOALS MENU", "What would you like to do?", ["Back", "Get All Goals", "Add Goal", "Delete Goal", "Get Completed Goals"]);

    switch (choice)
    {
      case "Back":
        break;
      case "Get All Goals":
        // GetAllGoals()
        break;
      case "Add Goal":
        // InsertGoal()
        break;
      case "Delete Goal":
        // DeleteGoal()
        break;
      case "Get Completed Goals":
        // GetCompletedGoals()
        break;
    }
  }

  private string[]? StartCoding()
  {
    AnsiConsole.Clear();
    Stopwatch.Start();
    AnsiConsole.Markup("Your coding session is [green]active[/].\n");
    string stop = ConsoleEngine.GetSelection("CODING SESSION", "Your coding session is [green]Active[/]. Press Enter when you are done.", ["Stop Coding"]);

    if (stop == "Stop Coding")
    {
      Stopwatch.Stop();

      string startDate = Stopwatch.StartDate.ToString("dd-MM-yy HH:mm");
      string endDate = Stopwatch.StopDate.ToString("dd-MM-yy HH:mm");
      return [startDate, endDate];
    }

    return null;
  }
}
