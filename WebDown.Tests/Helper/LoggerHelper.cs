using Moq;
using Microsoft.Extensions.Logging;

public static class LoggerHelper
{
    public static Mock<ILogger<T>> LoggerMock<T>() where T : class
    {
        return new Mock<ILogger<T>>();
    }

    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string containMessage, Times times)
    {
        loggerMock.Verify(
        x => x.Log(
            level,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(containMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
        times);
    }

    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Times times)
    {
        loggerMock.Verify(
        x => x.Log(
            level,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
        times);
    }
}

