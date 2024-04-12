using System.Globalization;

namespace CodingTracker.Database.Models
{
  public class CodingSession
  {
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Duration { get; set; }

    public CodingSession(int id, string startDate, string endDate)
    {
      Id = id;
      StartDate = DateTime.ParseExact(startDate, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
      EndDate = DateTime.ParseExact(endDate, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
      Duration = CalculateDuration();
    }

    public int CalculateDuration()
    {
      TimeSpan duration = EndDate - StartDate;
      return duration.Minutes;
    }
  }
}
