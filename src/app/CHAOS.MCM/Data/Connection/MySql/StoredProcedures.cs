namespace Chaos.Mcm.Data.Connection.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Chaos.Mcm.Data.Dto.Standard;

    using global::MySql.Data.MySqlClient;

    public class StoredProcedures
    {
        #region Fields

        private readonly string _connectionString;

        #endregion
        #region Initialization

        public StoredProcedures( string connectionString )
        {
            _connectionString = connectionString;
        }

        #endregion
        #region Properties

        public IList<TResultType> ExecuteQuery<TResultType>(string storedProcedure, IEnumerable<MySqlParameter> parameters)
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
        
        public long ExecuteNonQuery(string storedProcedure, IEnumerable<MySqlParameter> parameters)
        {
            using (var connnection = new MySqlConnection(_connectionString))
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
        #region Business Logic

        public IList<ObjectRelationInfo> ObjectRelationInfoGet(Guid objectGuid)
        {
            var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("Object1Guid", objectGuid.ToByteArray()),
                };

            return ExecuteQuery<ObjectRelationInfo>("ObjectRelationInfo_Get", parameters);
        }

        public long ObjectRelationCreate(ObjectRelation objectRelation)
        {
            var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("Object1Guid", objectRelation.Object1Guid.ToByteArray()),
                    new MySqlParameter("Object2Guid", objectRelation.Object2Guid.ToByteArray()),
                    new MySqlParameter("ObjectRelationTypeID", objectRelation.ObjectRelationTypeID),
                    new MySqlParameter("MetadataGuid", objectRelation.MetadataGuid.ToByteArray()),
                    new MySqlParameter("Sequence", objectRelation.Sequence)
                };

            return this.ExecuteNonQuery("ObjectRelation_Create",parameters);
        }

        #endregion
    }
}