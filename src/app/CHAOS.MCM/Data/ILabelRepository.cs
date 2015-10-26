using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;

namespace Chaos.Mcm.Data
{
	public interface ILabelRepository
	{
		bool AssociationWithProject(uint id, Guid objectId);
		bool DisassociationWithProject(uint id, Guid objectId);
		IEnumerable<Label> Get(uint projectId);
		Label Set(uint projectId, Label label);
	}
}