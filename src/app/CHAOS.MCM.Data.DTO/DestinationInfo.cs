using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geckon;
using Geckon.Portal.Data.Result.Standard;
using Geckon.Serialization;

namespace CHAOS.MCM.Data.DTO
{
	public class DestinationInfo : Result
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public UUID SubscriptionGUID { get; set; }

		[Serialize]
		public string Name { get; set; }

		[Serialize]
		public DateTime DateCreated { get; set; }

		[Serialize]
		public string BasePath { get; set; }

		[Serialize]
		public string StringFormat { get; set; }

		[Serialize]
		public string Token { get; set; }

		#endregion
		#region Construction

		public DestinationInfo( uint id, Guid subscriptionGUID, string name, string basePath, string stringFormat, string token, DateTime dateCreated)
		{
			ID               = id;
			SubscriptionGUID = new UUID( subscriptionGUID.ToByteArray() );
			Name             = name;
			BasePath         = basePath;
			StringFormat     = stringFormat;
			Token            = token;
			DateCreated      = dateCreated;
		}

		public DestinationInfo()
		{
			
		}

		#endregion
	}
}
