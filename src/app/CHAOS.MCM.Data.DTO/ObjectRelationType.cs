﻿using System;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public  class ObjectRelationType : Result
	{
		#region Properties

		[Serialize("ID")]
		public uint ID { get; set; }

		[Serialize("Name")]
		public string Name { get; set; }

		#endregion
		#region Constructors

		public ObjectRelationType( uint id, string name )
		{
			ID   = id;
			Name = name;
		}

		public ObjectRelationType()
		{
			
		}

		#endregion
	}
}