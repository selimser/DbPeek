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

            var baseText = rawString;
            if (rawString.Contains('['))
            {
                baseText = rawString.Replace("[", string.Empty);
            }

            if (rawString.Contains(']'))
            {
                baseText = rawString.Replace("]", string.Empty);
            }

            if (baseText.Contains('.'))
            {
                var schemaSplit = baseText.Split('.');
                return new Tuple<string, string>(schemaSplit[0], schemaSplit[1]);
            }
            else
            {
                return new Tuple<string, string>("dbo", baseText);
            }
        }
    }
}
