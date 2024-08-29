using Moq;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Flixer.Catalog.UnitTest.Helpers;

public static class MockHelper
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, Times times, string? regex = null) =>
        logger.Verify(m => m.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((x, y) => 
                    regex == null || Regex.IsMatch(x.ToString() ?? string.Empty, regex)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
}