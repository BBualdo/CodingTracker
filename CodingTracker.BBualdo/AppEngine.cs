using CodingTracker.Database;
using Spectre.Console;

namespace CodingTracker.BBualdo;

public class AppEngine
{
  public DbContext Db { get; set; }
  public bool IsRunning { get; set; }

  public AppEngine()
  {
    Db = new DbContext();
    IsRunning = true;
  }

  public void MainMenu()
  {
    string choice = ConsoleEngine.GetSelection("MAIN MENU", ["Start Coding", "Sessions", "Goals", "Close App"]);

    switch (choice)
    {
      case "Start Coding":
        // StartCoding()
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
    string choice = ConsoleEngine.GetSelection("SESSIONS MENU", ["Back", "Get All Sessions", "Insert Session", "Update Session", "Delete Session"]);

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
        // UpdateSession()
        break;
      case "Delete Session":
        // DeleteSession()
        break;
    }
  }

  public void GoalsMenu()
  {
    string choice = ConsoleEngine.GetSelection("GOALS MENU", ["Back", "Get All Goals", "Add Goal", "Delete Goal", "Get Completed Goals"]);

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
}
