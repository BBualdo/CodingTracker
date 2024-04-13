using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Configuration;

namespace CodingTracker.Database;

public class DbContext
{
  private readonly string _connectionString;
  private readonly SessionDataAccess _sessionDataAccess;

  public DbContext()
  {
    _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
    CreateTables();
    SeedData();
    _sessionDataAccess = new SessionDataAccess(_connectionString);
  }

  public bool DeleteSession()
  {
    AnsiConsole.Clear();

    _sessionDataAccess.DeleteSession();
    return true;
  }

  public bool UpdateSession()
  {
    AnsiConsole.Clear();

    _sessionDataAccess.UpdateSession();
    return true;
  }

  public bool InsertSession()
  {
    AnsiConsole.Clear();

    string startDate = UserInput.GetStartDate();
    string endDate = UserInput.GetEndDate(startDate);
    _sessionDataAccess.InsertSession(startDate, endDate);
    return true;
  }

  public bool InsertSession(string startDate, string endDate)
  {
    AnsiConsole.Clear();
    _sessionDataAccess.InsertSession(startDate, endDate);
    return true;
  }

  public bool GetAllSessions()
  {
    AnsiConsole.Clear();

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
                              finish_date TEXT,
                              target_duration TEXT,
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

      string countRecordsSql = "SELECT COUNT(*) FROM sessions";

      using (SqliteCommand countCommand = new SqliteCommand(countRecordsSql, connection))
      {
        int recordsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

        Random random = new Random();

        if (recordsNumber == 0)
        {
          Console.WriteLine("Loading...");

          for (int i = 0; i < 10; i++)
          {
            DateTime startDateTime = DateTime.Now.AddDays(-random.Next(0, 365)).AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));
            DateTime endDateTime = startDateTime.AddHours(random.Next(0, 7)).AddMinutes(random.Next(0, 60));

            TimeSpan durationTimeSpan = endDateTime - startDateTime;

            string startDate = startDateTime.ToString("dd-MM-yy HH:mm");
            string endDate = endDateTime.ToString("dd-MM-yy HH:mm");
            int duration = Convert.ToInt32(durationTimeSpan.TotalMinutes);

            string insertSql = $"INSERT INTO sessions(start_date, end_date, duration) VALUES('{startDate}', '{endDate}', {duration})";

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