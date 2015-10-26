using System;
using System.Collections.Generic;

namespace Chaos.Mcm.Data.Dto
{
	public class Project
	{
		public uint? Identifier { get; set; }
		public string Name { get; set; }
		public IList<Guid> UserIds { get; set; }
		public IList<Label> Labels { get; set; }

		public Project()
		{
			UserIds = new List<Guid>();
			Labels = new List<Label>();
		}
	}
}