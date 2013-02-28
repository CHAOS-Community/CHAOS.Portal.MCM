namespace Chaos.Mcm.Data.Dto
{
    using System;

    using CHAOS.Serialization;

    using Chaos.Portal.Data.Dto;

    public class DestinationInfo : AResult
	{
		#region Properties

		[Serialize]
		public uint ID { get; set; }

		[Serialize]
		public Guid SubscriptionGuid { get; set; }

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

		public DestinationInfo( uint id, Guid subscriptionGuid, string name, string basePath, string stringFormat, string token, DateTime dateCreated)
		{
			this.ID               = id;
			this.SubscriptionGuid = subscriptionGuid;
			this.Name             = name;
			this.BasePath         = basePath;
			this.StringFormat     = stringFormat;
			this.Token            = token;
			this.DateCreated      = dateCreated;
		}

		public DestinationInfo()
		{
			
		}

		#endregion
	}
}
