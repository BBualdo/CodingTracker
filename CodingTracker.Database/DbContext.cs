using Microsoft.Data.Sqlite;

namespace CodingTracker.Database;

public class DbContext
{
  private readonly SessionDataAccess _sessionDataAccess;

  public DbContext()
  {
    CreateTables();
    SeedData();
    _sessionDataAccess = new SessionDataAccess();
  }

  public bool GetAllSessions()
  {
    _
  }

  private void CreateTables()
  {
    using (SqliteConnection connection = new SqliteConnection(ConnectionString))
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
    using (SqliteConnection connection = new SqliteConnection(ConnectionString))
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
