﻿using Spectre.Console;
using System.Globalization;

namespace CodingTracker.Database.Helpers;

public class DateTimeValidator
{
  public static bool IsValid(string date)
  {
    if (DateTime.TryParseExact(date, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime temp))
    {
      if (temp < new DateTime(2000, 1, 1) || temp > DateTime.Now)
      {
        AnsiConsole.Markup("[red]Date and time must be in the past and can't be older than 01-01-00 00:00.[/] ");
        return false;
      }

      return true;
    };

    AnsiConsole.Markup("[red]Invalid date. Must be in format (dd-MM-yy HH:mm).[/] ");
    return false;
  }

  public static bool AreValid(string startDate, string endDate)
  {
    DateTime startDateTemp = DateTime.ParseExact(startDate, "dd-MM-yy HH:mm", new CultureInfo("en-US"));
    DateTime endDateTemp = DateTime.ParseExact(endDate, "dd-MM-yy HH:mm", new CultureInfo("en-US"));

    if (endDateTemp > startDateTemp)
    {
      return true;
    }

    AnsiConsole.Markup($"[red]End date and time can't be older than start date. Please enter date newer than {startDate}.[/] ");
    return false;
  }
}
