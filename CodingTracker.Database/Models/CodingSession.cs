using System.Globalization;

namespace CodingTracker.Database.Models
{
  public class CodingSession
  {
    public int Session_Id { get; set; }
    public string Start_Time { get; set; }
    public string End_Time { get; set; }
    public int Duration { get; set; }

    public CodingSession() { }

    public CodingSession(int session_Id, string start_Time, string end_Time)
    {
      Session_Id = session_Id;
      Start_Time = start_Time;
      End_Time = end_Time;
      Duration = CalculateDuration();
    }

    public int CalculateDuration()
    {
      TimeSpan duration = DateTime.ParseExact(End_Time, "dd-MM-yy HH:mm", new CultureInfo("en-US")) - DateTime.ParseExact(Start_Time, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
      return duration.Minutes;
    }
  }
}
