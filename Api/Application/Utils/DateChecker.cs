namespace Api.Application;

public static class DateChecker
{
    public static bool AreSameDate(DateTime dateOne, DateTime dateTwo)
    {
        return dateTwo.Year == dateOne.Year &&
                dateTwo.Month == dateOne.Month;
    }

    public static bool IsCurrent(DateTime date)
    {
        var currentDate = DateTime.Today;

        return AreSameDate(currentDate, date);
    }

    public static bool IsGreaterThanThisMonth(DateTime date)
    {
        var currentDate = DateTime.Today;

        return date.Year > currentDate.Year ||
                (date.Year == currentDate.Year &&
                    date.Month > currentDate.Month);
    }
}