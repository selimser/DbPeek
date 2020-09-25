using DbPeek.Services.Database;
using Xunit;

namespace DbPeek.Tests
{
    public class QueryServiceTest
    {
        private const string ExpectedStoredProcedureName = "TestProcedure";
        private const string ExpectedDefaultSchema = "dbo";
        private const string ExpectedCustomSchema = "api";

        [Fact]
        public void SelimTest()
        {
            Assert.Equal(1, 1);
        }

        [Theory]
        [InlineData("dbo.TestProcedure")]
        [InlineData("[dbo].[TestProcedure]")]
        public void QueryService_ValidSpName_CorrectSplit(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Item1);
            Assert.Equal(ExpectedStoredProcedureName, result.Item2);

        }

        [Theory]
        [InlineData("TestProcedure")]
        [InlineData("[TestProcedure]")]
        public void QueryService_SpNameWithoutSchema_DefaultDboPrepended(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Item1);
            Assert.Equal(ExpectedStoredProcedureName, result.Item2);
        }

        [Theory]
        [InlineData("[dbo.TestProcedure")]
        [InlineData("dbo].TestProcedure")]
        [InlineData("[dbo].TestProcedure")]
        [InlineData("dbo.[TestProcedure")]
        [InlineData("dbo.TestProcedure]")]
        [InlineData("dbo.[TestProcedure]")]
        public void QueryService_IncorrectBracketPlacementWithDefaultSchema_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Item1);
            Assert.Equal(ExpectedStoredProcedureName, result.Item2);
        }

        [Theory]
        [InlineData("api.TestProcedure")]
        [InlineData("[api].[TestProcedure]")]
        [InlineData("[api].TestProcedure")]
        [InlineData("api.[TestProcedure]")]
        public void QueryService_CustomSchema_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedCustomSchema, result.Item1);
            Assert.Equal(ExpectedStoredProcedureName, result.Item2);
        }

        [Theory]
        [InlineData("[api.TestProcedure")]
        [InlineData("api].TestProcedure")]
        [InlineData("[api].TestProcedure")]
        [InlineData("api.[TestProcedure")]
        [InlineData("api.TestProcedure]")]
        [InlineData("api.[TestProcedure]")]
        public void QueryService_CustomSchemaWithIncorrectBrackets_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedCustomSchema, result.Item1);
            Assert.Equal(ExpectedStoredProcedureName, result.Item2);
        }

        [Theory]
        [InlineData("dbo.Test.Procedure")]
        public void QueryService_DefaultSchema_NameContainsDots(string input)
        {

        }
    }
}
