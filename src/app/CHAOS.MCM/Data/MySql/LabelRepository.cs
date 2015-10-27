using System;
using System.Collections.Generic;
using CHAOS.Data.MySql;
using Chaos.Mcm.Data.Dto;
using MySql.Data.MySqlClient;

namespace Chaos.Mcm.Data.MySql
{
	public class LabelRepository : ILabelRepository
	{
		public Gateway Gateway { get; set; }

		public LabelRepository(Gateway gateway)
		{
			Gateway = gateway;
		}

		public bool AssociationWithProject(uint id, Guid objectId)
		{
			var result = Gateway.ExecuteNonQuery("Label_Object_Join_Set", new MySqlParameter("LabelId", id),
																					 new MySqlParameter("ObjectId", objectId.ToByteArray()));

			return result == 1;
		}

		public bool DisassociationWithProject(uint id, Guid objectId)
		{
			var result = Gateway.ExecuteNonQuery("Label_Object_Join_Delete", new MySqlParameter("LabelId", id),
			                                     new MySqlParameter("ObjectId", objectId.ToByteArray()));

			return result == 1;
		}

		public IEnumerable<Label> Get(uint? projectId = null, Guid? objectId = null)
		{
			var results = Gateway.ExecuteQuery("Label_Get", new MySqlParameter("ProjectId", projectId),
			                                   new MySqlParameter("ObjectId", objectId.HasValue ? objectId.Value.ToByteArray() : null));

			foreach (var result in results)
			{
				yield return new Label
					{
						Identifier = (uint) result.Id,
						Name = result.Name
					};
			}
		}

		public Label Set(uint projectId, Label label)
		{
			var id = Gateway.ExecuteNonQuery("Label_Set", new MySqlParameter("Id", label.Identifier),
			                                 new MySqlParameter("ProjectId", projectId),
			                                 new MySqlParameter("Name", label.Name));

			label.Identifier = (uint?) id;

			return label;
		}

		public bool Delete(uint id)
		{
			var result = Gateway.ExecuteNonQuery("Label_Delete", new MySqlParameter("Id", id));

			return result > 0;
		}
	}
}