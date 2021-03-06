using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Data.MySql;
using Chaos.Mcm.Data.Dto;
using Chaos.Mcm.Data.Mapping;
using CHAOS.Data;
using MySql.Data.MySqlClient;

namespace Chaos.Mcm.Data.MySql
{
	public class ProjectRepository : IProjectRepository
	{
		private Gateway Gateway { get; set; }
		public ILabelRepository Label { get; set; }

		static ProjectRepository()
		{
			ReaderExtensions.Mappings.Add(typeof (Project), new ProjectMapping());
		}

		public ProjectRepository(Gateway gateway, ILabelRepository label)
		{
			Gateway = gateway;
			Label = label;
		}

		public Project Set(Project project)
		{
			var result = Gateway.ExecuteNonQuery("Project_Set",
				new MySqlParameter("Id",
					project.Identifier.HasValue
						? project.Identifier.Value.ToString()
						: null),
				new MySqlParameter("Name", project.Name));

			project.Identifier = (uint) result;

			return project;
		}

		public IEnumerable<Project> Get(uint? id = null, Guid? userId = null, uint? labelId = null)
		{
			return Gateway.ExecuteQuery<Project>("Project_Get",
				new MySqlParameter("Id", id.HasValue ? id.Value.ToString() : null),
				new MySqlParameter("LabelId", labelId.HasValue ? labelId.Value.ToString() : null),
				new MySqlParameter("UserId", userId.HasValue ? userId.Value.ToByteArray() : null));
		}

		public bool Delete(uint id)
		{
			var result = Gateway.ExecuteNonQuery("Project_Delete", new MySqlParameter("Id", id));

			return result > 0;
		}

		public bool AddUser(uint id, Guid userId)
		{
			var result = Gateway.ExecuteNonQuery("Project_User_Join_Set",
				new MySqlParameter("ProjectId", id),
				new MySqlParameter("UserId", userId.ToByteArray()));

			return result == 1;
		}

		public bool RemoveUser(uint id, Guid userId)
		{
			var result = Gateway.ExecuteNonQuery("Project_User_Join_Delete",
				new MySqlParameter("ProjectId", id),
				new MySqlParameter("UserId", userId.ToByteArray()));

			return result == 1;
		}
	}
}