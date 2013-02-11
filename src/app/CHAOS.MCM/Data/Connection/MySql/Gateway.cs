namespace Chaos.Mcm.Data.Connection.MySql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::MySql.Data.MySqlClient;

    public class Gateway
    {
        #region Fields

        private readonly string _connectionString;

        #endregion
        #region Initialization

        public Gateway( string connectionString )
        {
            _connectionString = connectionString;
        }

        #endregion
        #region Properties

        #endregion

        #region Business Logic

        public IList<TResultType> ExecuteQuery<TResultType>(string storedProcedure, params MySqlParameter[] parameters)
        {
            var s = new System.Diagnostics.Stopwatch();
            s.Start();
            
            using (var connnection = new MySqlConnection(this._connectionString))
            using (var command     = new MySqlCommand())
            {
                command.Connection  = connnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters.ToArray());

                connnection.Open();

                using (var reader = command.ExecuteReader())
                {
                    return reader.Map<TResultType>().Select(item => item).ToList();
                }
            }
        }

        public long ExecuteNonQuery(string storedProcedure, params MySqlParameter[] parameters)
        {
            using (var connnection = new MySqlConnection(this._connectionString))
            using (var command     = new MySqlCommand())
            {
                command.Connection  = connnection;
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(parameters.ToArray());

                connnection.Open();

                return System.Convert.ToInt64(command.ExecuteScalar());
            }
        }

        #endregion
    }
}