using DbPeek.Helpers.Editor;
using Xunit;

namespace DbPeek.Tests.Services
{
    public class FileServiceTest
    {
        [Theory]
        [InlineData(0, 2, "0.00 bytes")]
        [InlineData(120, 0, "120 bytes")]
        [InlineData(120, 2, "120.00 bytes")]
        [InlineData(120469, 4, "117.6455 KB")]
        [InlineData(1200, 2, "1.17 KB")]
        [InlineData(12000, 2, "11.72 KB")]
        [InlineData(120000, 2, "117.19 KB")]
        [InlineData(1200000, 2, "1.14 MB")]
        [InlineData(12000000, 2, "11.44 MB")]
        [InlineData(120000000, 2, "114.44 MB")]
        [InlineData(1200000000, 2, "1.12 GB")]
        [InlineData(12000000000, 2, "11.18 GB")]
        public void FileService_CalculateSizeSuffix_Success(long sizeInBytes, uint precision, string expectedResult)
        {
            var calculatedValue = FileService.CalculateSizeSuffix(sizeInBytes, precision);

            Assert.Equal(expectedResult, calculatedValue);
        }
    }
}
