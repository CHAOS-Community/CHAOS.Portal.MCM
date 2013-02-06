namespace Chaos.Mcm.Data.Connection.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;

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

        public IList<TResultType> ExecuteQuery<TResultType>(string storedProcedure, params MySqlParameter[] parameters) where TResultType : IKeyValueMapper, new()
        {
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
                    var list = new List<KeyValuePair<string, object>[]>();

                    while (reader.Read())
                    {
                        var row = new KeyValuePair<string, object>[reader.FieldCount];

                        for (var i = 0; i < reader.FieldCount; i++)
                            row[i] = new KeyValuePair<string, object>(reader.GetName(i), reader.GetValue(i));

                        list.Add(row);
                    }

                    return list.ToDto<TResultType>();
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
                
                return (long) command.ExecuteScalar();
            }
        }

        #endregion
    }
}