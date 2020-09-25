using DbPeek.Models;
using Xunit;

namespace DbPeek.Tests.Models
{
    public class ErrorDataTest
    {
        [Fact]
        public void ErrorData_ContainsErrors_GeneratesMessage()
        {
            var subject = new ErrorData
            {
                ErrorCode = ErrorCodes.GenericException
            };

            Assert.Equal("An error has occurred.\n1000 - GenericException", subject.ErrorMessage);
        }
    }
}
