using System.Collections.Generic;
using System.Data;
using System.Linq;
using Chaos.Mcm.Data.Dto;
using CHAOS.Data;
using CHAOS.Extensions;

namespace Chaos.Mcm.Data.Mapping
{
	public class ProjectMapping : IReaderMapping<Project>
	{
		public IEnumerable<Project> Map(IDataReader reader)
		{
			var projects = new Dictionary<uint, Project>();
			
			while (reader.Read())
			{
				var id = reader.GetUint32("Id");

				projects.Add(id,
					new Project
					{
						Identifier = id,
						Name = reader.GetString("Name")
					});
			}

			reader.NextResult();

			while (reader.Read())
			{
				var projectId = reader.GetUint32("ProjectId");
				var project = projects[projectId];
				var labelId = reader.GetUint32Nullable("Id");

				project.Labels.Add(new Label
				{
					Identifier = labelId,
					Name = reader.GetString("Name"),
					ProjectId = projectId
				});
			}

			reader.NextResult();

			while (reader.Read())
			{
				var projectId = reader.GetUint32("ProjectId");
				var project = projects[projectId];
				var userId = reader.GetGuidNullable("UserId");

				if (userId != null) project.UserIds.Add(userId.Value);
			}

			return projects.Values;
		}
	}
}