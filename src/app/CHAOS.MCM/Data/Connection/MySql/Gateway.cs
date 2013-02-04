namespace Chaos.Mcm.Data.Connection.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Chaos.Mcm.Data.Dto.Standard;

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

        public IList<ObjectRelationInfo> ObjectRelationInfoGet(Guid objectGuid)
        {
            var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("Object1Guid", objectGuid.ToByteArray()),
                };

            return ExecuteQuery<ObjectRelationInfo>("ObjectRelationInfo_Get", parameters);
        }

        public long ObjectRelationSet(ObjectRelation objectRelation)
        {
            var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("Object1Guid", objectRelation.Object1Guid.ToByteArray()),
                    new MySqlParameter("Object2Guid", objectRelation.Object2Guid.ToByteArray()),
                    new MySqlParameter("ObjectRelationTypeID", objectRelation.ObjectRelationTypeID),
                    new MySqlParameter("Sequence", objectRelation.Sequence)
                };

            return this.ExecuteNonQuery("ObjectRelation_Set", parameters);
        }

        public long ObjectRelationSetMetadata(ObjectRelationInfo objectRelation, Guid editintUserGuid)
        {
            var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("Object1Guid", objectRelation.Object1Guid.ToByteArray()),
                    new MySqlParameter("Object2Guid", objectRelation.Object2Guid.ToByteArray()),
                    new MySqlParameter("ObjectRelationTypeID", objectRelation.ObjectRelationTypeID),
                    new MySqlParameter("Sequence", objectRelation.Sequence),
                    new MySqlParameter("MetadataGuid", objectRelation.MetadataGuid.Value.ToByteArray()),
                    new MySqlParameter("MetadataSchemaGuid", objectRelation.MetadataSchemaGuid.Value.ToByteArray()),
                    new MySqlParameter("MetadataXml", objectRelation.MetadataXml),
                    new MySqlParameter("LanguageCode", objectRelation.LanguageCode),
                    new MySqlParameter("EditingUserGuid", editintUserGuid.ToByteArray()),
                };

            return this.ExecuteNonQuery("ObjectRelation_SetMetadata", parameters);
        }

        #endregion
    }
}