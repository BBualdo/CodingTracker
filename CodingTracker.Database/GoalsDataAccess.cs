using CodingTracker.Database.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Database;

public class GoalsDataAccess
{
  private readonly string _connectionString;

  public GoalsDataAccess(string connectionString)
  {
    _connectionString = connectionString;
  }

  public List<Goal> GetAllGoals(bool filterByCompleted)
  {
    List<Goal> goals = new List<Goal>();

    using (SqliteConnection connection = new SqliteConnection(_connectionString))
    {
      connection.Open();

      string selectSql;

      if (!filterByCompleted)
      {
        selectSql = "SELECT * FROM goals";
      }
      else
      {
        selectSql = "SELECT * FROM goals WHERE is_completed=1";
      }

      goals = connection.Query<Goal>(selectSql).ToList();
    }

    return goals;
  }
}
