using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;

namespace Chaos.Mcm.Data
{
	public interface IProjectRepository
	{
		Project Set(Project project);
		IEnumerable<Project> Get(uint? id = null, Guid? userId = null, uint? labelId = null);
		bool AddUser(uint id, Guid userId);
		bool RemoveUser(uint id, Guid userId);
	}
}