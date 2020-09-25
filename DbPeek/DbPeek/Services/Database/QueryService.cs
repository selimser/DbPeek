using DbPeek.Extensions;
using DbPeek.Models;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DbPeek.Tests", AllInternalsVisible = true)]
namespace DbPeek.Services.Database
{
    internal static class QueryService
    {
        internal static ParseStoredProcedureResult ParseStoredProcedureName(string rawString)
        {
            var result = new ParseStoredProcedureResult();

            var validationResult = ValidateSpName(rawString, out var parsedValue);
            if (!validationResult.IsNameValid)
            {
                result.Error = new ErrorData
                {
                    ErrorCode = (ErrorCodes)validationResult.ErrorCode
                };
            }
            else
            {
                result.Schema = parsedValue.Item1;
                result.Name = parsedValue.Item2;
            }

            return result;
        }

        private static (bool IsNameValid, ErrorCodes? ErrorCode) ValidateSpName(string rawString, out Tuple<string, string> parsedValue)
        {
            var trimmedSpName = rawString.TrimStart().TrimEnd();
            var containsInvalidChars = trimmedSpName.ContainsAny(new[] { ",", ";", ":", "\"", "/", "'" });

            if (containsInvalidChars)
            {
                parsedValue = null;
                return (false, ErrorCodes.InvalidCharacters);
            }

            var filteredSpName = trimmedSpName
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Split('.');

            if (filteredSpName.Length == 1) //without a schema
            {
                parsedValue = new Tuple<string, string>("dbo", filteredSpName[0]);
                return (true, null);
            }
            else if (filteredSpName.Length == 2) //with a schema
            {
                parsedValue = new Tuple<string, string>(filteredSpName[0], filteredSpName[1]);
                return (true, null);
            }
            else //contains any other dots other than schema
            {
                parsedValue = null;
                return (false, ErrorCodes.InvalidCharacters);
            }
        }
    }
}
