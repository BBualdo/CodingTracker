using CodingTracker.Database.Helpers;
using CodingTracker.Database.Models;
using Dapper;
using Microsoft.Data.Sqlite;

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
}
