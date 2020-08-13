using DbPeek.Services.Settings;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DbPeek.Services.Database
{
    internal sealed class SpUtilsService
    {
        private static SpUtilsService _instance;
        private static readonly object _lock = new object();

        internal static SpUtilsService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SpUtilsService();
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Each template for specific version will go here (e.g. support for older versions)
        /// </summary>
        internal struct CommandTempaltes
        {
            internal const string GetContents = "EXEC sp_helptext '[{0}].[{1}]';";
        }

        internal async Task<string> GetStoredProcedureAsync(string storedProcedureName)
        {
            var procedureTuple = QueryService.ParseStoredProcedureName(storedProcedureName);

            var parsedCommand =
                string.Format
                (
                    CommandTempaltes.GetContents,
                    procedureTuple.Item1,
                    procedureTuple.Item2
                ); //need to check if the sp exists? TODO.

            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = VsShellSettingsService.ReadSetting<string>("TargetConnectionString");

                using (var command = new SqlCommand(parsedCommand))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Connection = connection;

                    var contentBuilder = new StringBuilder();

                    await connection.OpenAsync();
                    using (var sqlReader = await command.ExecuteReaderAsync())
                    {
                        if (sqlReader.HasRows)
                        {
                            while (await sqlReader.ReadAsync())
                            {
                                contentBuilder.Append(sqlReader.GetString(0));
                            }
                        }

                        sqlReader.Close();
                    }

                    connection.Close();

                    var contentesult = contentBuilder.ToString();
                    return contentesult;
                }
            }
        }
    }
}
