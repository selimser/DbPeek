using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DbPeek.Tests", AllInternalsVisible = true)]
namespace DbPeek.Services.Database
{
    internal static class QueryService
    {
        internal static Tuple<string, string> ParseStoredProcedureName(string rawString)
        {
            var filteredSpName = rawString
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace(" ", string.Empty)
                .Replace(",", string.Empty)
                .Replace("'", string.Empty)
                .Replace(":", string.Empty)
                .Replace(";", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("/", string.Empty);

            if (filteredSpName.Contains('.')) //TODO: what if it contains multiple dots?
            {
                var schemaSplit = filteredSpName.Split('.');
                return new Tuple<string, string>(schemaSplit[0], schemaSplit[1]);
            }
            else
            {
                return new Tuple<string, string>("dbo", filteredSpName);
            }
        }
    }
}
