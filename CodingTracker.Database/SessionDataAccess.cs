using CodingTracker.Database.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.Database;

public class SessionDataAccess
{
  private readonly string _connectionString;

  public SessionDataAccess()
  {
    _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
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
}
