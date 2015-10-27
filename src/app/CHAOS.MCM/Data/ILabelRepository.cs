using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;

namespace Chaos.Mcm.Data
{
	public interface ILabelRepository
	{
		bool AssociationWithObject(uint id, Guid objectId);
		bool DisassociationWithObject(uint id, Guid objectId);
		IEnumerable<Label> Get(uint? projectId = null, Guid? objectId = null);
		Label Set(uint projectId, Label label);
		bool Delete(uint id);
	}
}