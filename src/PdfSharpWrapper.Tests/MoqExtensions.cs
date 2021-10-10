using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace PdfSharpWrapper.Tests
{
    public static class MoqExtensions
    {
        public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, Func<Times> times)
        {
            logger.Verify(
                x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), times);

            return logger;
        }

        public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel, Func<Times> times)
        {
            Func<object, Type, bool> state = (v, t) => true;

            logger.VerifyLogging(state, expectedLogLevel, times);

            return logger;
        }

        public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel, Func<Times> times)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().Equals(expectedMessage);

            logger.VerifyLogging(state, expectedLogLevel, times);

            return logger;
        }

        private static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, Func<object, Type, bool> state, LogLevel expectedLogLevel, Func<Times> times)
        {
            logger.Verify(
                x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), times);

            return logger;
        }
    }
}
