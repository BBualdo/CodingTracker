using CodingTracker.Database.enums;
using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;

namespace CodingTracker.Database.DbContext;

public class DbContext
{
  private readonly string _connectionString;
  private readonly SessionDataAccess _sessionDataAccess;
  private readonly GoalsDataAccess _goalsDataAccess;

  public DbContext()
  {
    _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    CreateTables();
    SeedData();
    _sessionDataAccess = new SessionDataAccess(_connectionString);
    _goalsDataAccess = new GoalsDataAccess(_connectionString);
  }

  public bool DeleteGoal()
  {
    _goalsDataAccess.DeleteGoal();
    return true;
  }

  public bool GetAllGoals(bool filterByCompleted = false)
  {
    List<Goal> goals = _goalsDataAccess.GetAllGoals(filterByCompleted);

    ConsoleEngine.GetGoalsTable(goals);

    AnsiConsole.WriteLine("Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool GetReport(ReportOptions reportOption, OrderOptions? orderOption)
  {
    List<CodingSession> sessions = _sessionDataAccess.GetAllSessions(reportOption);
    List<CodingSession> orderedSessions;
    if (orderOption == OrderOptions.ASC)
    {
      orderedSessions = sessions.OrderBy(session => session.Duration).ToList();
    }
    else if (orderOption == OrderOptions.DESC)
    {
      orderedSessions = sessions.OrderByDescending(session => session.Duration).ToList();
    }
    else
    {
      orderedSessions = sessions;
    }

    ConsoleEngine.GetCodingSessionsTable(orderedSessions);

    ConsoleEngine.GetDurationSummary(orderedSessions, reportOption);

    AnsiConsole.WriteLine("Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  public bool DeleteSession()
  {
    _sessionDataAccess.DeleteSession();
    return true;
  }

  public bool UpdateSession()
  {
    _sessionDataAccess.UpdateSession();
    return true;
  }

  public bool InsertSession()
  {
    string startDate = UserInput.GetStartDate();
    string endDate = UserInput.GetEndDate(startDate);
    _sessionDataAccess.InsertSession(startDate, endDate);
    return true;
  }

  public bool InsertSession(string startDate, string endDate)
  {
    _sessionDataAccess.InsertSession(startDate, endDate);
    return true;
  }

  public bool GetAllSessions()
  {
    List<CodingSession> sessions = _sessionDataAccess.GetAllSessions();

    ConsoleEngine.GetCodingSessionsTable(sessions);

    AnsiConsole.WriteLine("Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }

  private void CreateTables()
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string createTablesSql = @"CREATE TABLE IF NOT EXISTS sessions(
                              session_id INTEGER PRIMARY KEY AUTOINCREMENT,
                              start_date TEXT,
                              end_date TEXT,
                              duration INT);
                              CREATE TABLE IF NOT EXISTS goals(
                              goal_id INTEGER PRIMARY KEY AUTOINCREMENT,
                              start_date TEXT,
                              end_date TEXT,
                              target_duration INTEGER,
                              is_completed INTEGER)";

      using (SqliteCommand createCommand = new SqliteCommand(createTablesSql, connection))
      {
        createCommand.ExecuteNonQuery();
      }
    }
  }

  private void SeedData()
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string countSessionsSql = "SELECT COUNT(*) FROM sessions";
      string countGoalsSql = "SELECT COUNT(*) FROM goals";

      Random random = new Random();

      using (SqliteCommand countCommand = new SqliteCommand(countSessionsSql, connection))
      {
        int recordsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

        if (recordsNumber == 0)
        {
          Console.WriteLine("Loading sessions...");

          for (int i = 0; i < 10; i++)
          {
            DateTime startDateTime = DateTime.Now.AddDays(-random.Next(0, 365)).AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));
            DateTime endDateTime = startDateTime.AddHours(random.Next(0, 7)).AddMinutes(random.Next(0, 60));

            TimeSpan durationTimeSpan = endDateTime - startDateTime;

            string startDate = startDateTime.ToString("yyyy-MM-dd HH:mm");
            string endDate = endDateTime.ToString("yyyy-MM-dd HH:mm");
            int duration = Convert.ToInt32(durationTimeSpan.TotalMinutes);

            string insertSql = $"INSERT INTO sessions(start_date, end_date, duration) VALUES('{startDate}', '{endDate}', {duration})";

            using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
            {
              insertCommand.ExecuteNonQuery();
            }
          }
        }
      }
      using (SqliteCommand countCommand = new SqliteCommand(countGoalsSql, connection))
      {
        int recordsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

        if (recordsNumber == 0)
        {
          Console.WriteLine("Loading goals...");

          for (int i = 0; i < 10; i++)
          {
            DateTime startDateTime = DateTime.Now.AddDays(-random.Next(0, 365)).AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));
            DateTime endDateTime = startDateTime.AddHours(random.Next(0, 7)).AddMinutes(random.Next(0, 60));

            string startDate = startDateTime.ToString("yyyy-MM-dd HH:mm");
            string endDate = endDateTime.ToString("yyyy-MM-dd HH:mm");
            int targetDuration = random.Next(60, 361);
            int isCompleted = random.Next(0, 2);

            string insertSql = $"INSERT INTO goals(start_date, end_date, target_duration, is_completed) VALUES('{startDate}', '{endDate}', {targetDuration}, {isCompleted})";

            using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
            {
              insertCommand.ExecuteNonQuery();
            }
          }
        }
      }
    }

    Console.Clear();
  }
}