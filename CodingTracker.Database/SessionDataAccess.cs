using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.Database;

public class SessionDataAccess
{
  private readonly string _connectionString;
  public SessionDataAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public List<CodingSession> GetAllSessions()
  {
    List<CodingSession> sessions = new List<CodingSession>();

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = "SELECT * FROM sessions";

      sessions = connection.Query<CodingSession>(selectSql).ToList();
    }

    return sessions;
  }

  public void InsertSession()
  {
    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string startDate = UserInput.GetStartDate();
      string endDate = UserInput.GetEndDate(startDate);
      int duration = DateTimeHelper.CalculateDuration(startDate, endDate);

      string insertSql = $"INSERT INTO sessions(start_date, end_date, duration) VALUES('{startDate}', '{endDate}', {duration})";

      using (SqliteCommand insertCommand = new SqliteCommand(insertSql, connection))
      {
        insertCommand.ExecuteNonQuery();
      }
    }
  }

  public bool UpdateSession()
  {
    List<CodingSession> sessions = GetAllSessions();
    ConsoleEngine.GetCodingSessionsTable(sessions);
    int id = AnsiConsole.Ask<int>("Type [green]ID[/] of the session you want to update: ");

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql = $"SELECT EXISTS(SELECT 1 FROM sessions WHERE session_id={id})";

      using (SqliteCommand selectCommand = new SqliteCommand(selectSql, connection))
      {
        if (Convert.ToInt32(selectCommand.ExecuteScalar()) == 0)
        {
          AnsiConsole.Markup("[red]Session with given id doesn't exists.[/] Press any key to return to Main Menu.");
          Console.ReadKey();
          return false;
        }
      }

      string startDate = UserInput.GetStartDate();
      string endDate = UserInput.GetEndDate(startDate);
      int duration = DateTimeHelper.CalculateDuration(startDate, endDate);

      string updateSql = $"UPDATE sessions SET start_date='{startDate}', end_date='{endDate}', duration={duration} WHERE session_id={id}";

      using (SqliteCommand updateCommand = new SqliteCommand(updateSql, connection))
      {
        updateCommand.ExecuteNonQuery();
      }
    }

    AnsiConsole.Markup("[green]Update Completed.[/] Press any key to return to Main Menu.");
    Console.ReadKey();
    return true;
  }
}
