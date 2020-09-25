using DbPeek.Models;
using DbPeek.Services.Database;
using Xunit;

namespace DbPeek.Tests.Services
{
    public class QueryServiceTest
    {
        private const string ExpectedStoredProcedureName = "TestProcedure";
        private const string ExpectedDefaultSchema = "dbo";
        private const string ExpectedCustomSchema = "api";

        [Theory]
        [InlineData("dbo.TestProcedure")]
        [InlineData("[dbo].[TestProcedure]")]
        public void QueryService_ParseSpNameWithValidSpName_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("TestProcedure")]
        [InlineData("[TestProcedure]")]
        public void QueryService_ParseSpNameWithoutSchema_DefaultSchemaPrepended(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("[dbo.TestProcedure")]
        [InlineData("dbo].TestProcedure")]
        [InlineData("[dbo].TestProcedure")]
        [InlineData("dbo.[TestProcedure")]
        [InlineData("dbo.TestProcedure]")]
        [InlineData("dbo.[TestProcedure]")]
        public void QueryService_ParseSpNameWithIncorrectBrackesAndDefaultSchema_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("api.TestProcedure")]
        [InlineData("[api].[TestProcedure]")]
        [InlineData("[api].TestProcedure")]
        [InlineData("api.[TestProcedure]")]
        public void QueryService_ParseSpNameWithCustomSchema_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedCustomSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("[api.TestProcedure")]
        [InlineData("api].TestProcedure")]
        [InlineData("[api].TestProcedure")]
        [InlineData("api.[TestProcedure")]
        [InlineData("api.TestProcedure]")]
        [InlineData("api.[TestProcedure]")]
        public void QueryService_ParseSpNameWithCustomSchemaAndIncorrectBrackets_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedCustomSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("[dbo].[TestProcedure] ")]
        [InlineData(" [dbo].[TestProcedure]")]
        [InlineData(" [dbo].[TestProcedure] ")]
        public void QueryService_ParseSpNameWithTrailingWhiteSpace_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("[api].[TestProcedure] ")]
        [InlineData(" [api].[TestProcedure]")]
        [InlineData(" [api].[TestProcedure] ")]
        public void QueryService_ParseSpNameWithTrailingWhiteSpaceAndCustomSchema_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedCustomSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("[TestProcedure] ")]
        [InlineData(" [TestProcedure]")]
        [InlineData(" [TestProcedure] ")]
        public void QueryService_ParseSpNameWithTrailingWhiteSpaceWithoutSchema_CorrectFormatting(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Equal(ExpectedDefaultSchema, result.Schema);
            Assert.Equal(ExpectedStoredProcedureName, result.Name);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("dbo.Test,Procedure")]
        [InlineData("dbo.Test;Procedure")]
        [InlineData("dbo.Test:Procedure")]
        [InlineData("dbo.Test\"Procedure")]
        [InlineData("dbo.Test/Procedure")]
        public void QueryService_ParseSpnameWithInvalidCharacters_ReturnsError(string input)
        {
            var result = QueryService.ParseStoredProcedureName(input);

            Assert.Null(result.Schema);
            Assert.Null(result.Name);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Equal(ErrorCodes.InvalidCharacters, result.Error.ErrorCode);
        }
    }
}
