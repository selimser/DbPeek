using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace DbPeek.Helpers.Database
{
    internal sealed class SpHelper
    {
        private static SpHelper _instance;
        private static readonly object _lock = new object();

        internal static SpHelper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SpHelper();
                    }

                    return _instance;
                }
            }
        }

        internal struct CommandTempaltes
        {
            internal const string GetContents = "EXEC [{0}].[{1}];";
        }

        internal async Task<string> GetStoredProcedure(string storedProcedureName)
        {
            var procedureTuple = QueryHelper.ParseStoredProcedureName(storedProcedureName);

            var parsedCommand =
                string.Format
                (
                    CommandTempaltes.GetContents,
                    procedureTuple.Item1,
                    procedureTuple.Item2
                ); //need to check if the sp exists?

            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = SettingsHelper.ReadSetting<string>("TargetConnectionString");

                using (var command = new SqlCommand(parsedCommand))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Connection = connection;


                    var contentBuilder = new StringBuilder();

                    await connection.OpenAsync();
                    using (var sqlReader = await command.ExecuteReaderAsync())
                    {
                        if (sqlReader.HasRows)
                        {
                            while (await sqlReader.ReadAsync())
                            {
                                contentBuilder.AppendLine(sqlReader.GetString(0));
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
