using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Data.MySql;
using Chaos.Mcm.Data.Dto;
using MySql.Data.MySqlClient;

namespace Chaos.Mcm.Data.MySql
{
	public class ProjectRepository : IProjectRepository
	{
		private Gateway Gateway { get; set; }
		public ILabelRepository Label { get; set; }

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
			var results = Gateway.ExecuteQuery("Project_Get",
			                                   new MySqlParameter("Id", id),
			                                   new MySqlParameter("LabelId", labelId),
			                                   new MySqlParameter("UserId", userId.HasValue ? userId.Value.ToByteArray() : null));

			foreach (var result in results)
				yield return new Project
					{
						Identifier = result.Id, 
						Name = result.Name, 
						Labels = Label.Get((uint)result.Id).ToList() // To improve performance this could be moved to the Stored procedure
					};
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