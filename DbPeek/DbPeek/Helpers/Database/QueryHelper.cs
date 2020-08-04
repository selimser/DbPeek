using System;
using System.Linq;

namespace DbPeek.Helpers.Database
{
    internal static class QueryHelper
    {
        //validate the query (e.g. schema, matching brackets and so on).
        //if fails, return false and show message to user.


        internal static Tuple<string, string> ParseStoredProcedureName(string rawString)
        {
            //check if it contains brackets already.

            //change it to something like this:
            //Input.Replace("(", "").Replace(")", "").Replace("-", "");


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

            //var baseText = rawString;
            //if (baseText.Contains('['))
            //{
            //    baseText = baseText.Replace("[", string.Empty);
            //}

            //if (baseText.Contains(']'))
            //{
            //    baseText = baseText.Replace("]", string.Empty);
            //}

            //if (baseText.Contains(' '))
            //{
            //    baseText = baseText.Replace(" ", string.Empty);
            //}

            //if (baseText.Contains(','))
            //{
            //    baseText = baseText.Replace(",", string.Empty);
            //}

            //if (baseText.Contains('\''))
            //{
            //    baseText = baseText.Replace("'", string.Empty);
            //}

            //if (baseText.Contains(':'))
            //{
            //    baseText = baseText.Replace(":", string.Empty);
            //}

            //if (baseText.Contains(';'))
            //{
            //    baseText = baseText.Replace(";", string.Empty);
            //}

            if (filteredSpName.Contains('.'))
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
