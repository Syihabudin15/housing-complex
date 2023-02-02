namespace HousingComplex.Exceptions;

public class MeetingStatusNotTrueException : Exception
{
    public MeetingStatusNotTrueException()
    {
    }

    public MeetingStatusNotTrueException(string? message) : base(message)
    {
    }
}