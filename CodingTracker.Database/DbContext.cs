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

  public bool GetAllSessions()
  {
    AnsiConsole.Clear();

    List<CodingSession> sessions = _sessionDataAccess.GetAllSessions();

    if (sessions.Count == 0)
    {
      AnsiConsole.WriteLine("Sessions list is empty. Create one first. Press any key to return to Main Menu.");
      Console.ReadKey();
      return false;
    }

    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Start Date"));
    table.AddColumn(new TableColumn("End Date"));
    table.AddColumn(new TableColumn("Duration"));

    foreach (CodingSession session in sessions)
    {
      table.AddRow(session.Session_Id.ToString(), session.Start_Time.ToString(), session.End_Time.ToString(), session.Duration.ToString());
    }

    AnsiConsole.Write(table);

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
                              start_time TEXT,
                              end_time TEXT,
                              duration INT);
                              CREATE TABLE IF NOT EXISTS goals(
                              goal_id INTEGER PRIMARY KEY AUTOINCREMENT,
                              start_time TEXT,
                              finish_time TEXT,
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

            string startTime = startDateTime.ToString("dd-MM-yy HH:mm");
            string endTime = endDateTime.ToString("dd-MM-yy HH:mm");
            int duration = Convert.ToInt32(durationTimeSpan.TotalMinutes);

            string insertSql = $"INSERT INTO sessions(start_time, end_time, duration) VALUES('{startTime}', '{endTime}', {duration})";

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
