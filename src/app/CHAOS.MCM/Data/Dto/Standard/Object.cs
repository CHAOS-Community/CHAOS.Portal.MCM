using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CHAOS;
using CHAOS.Index;
using CHAOS.Serialization;
using Chaos.Portal.Data.Dto.Standard;

namespace Chaos.Mcm.Data.Dto.Standard
{
	public class Object : Result, IIndexable
	{
		#region Properties

		[Serialize("GUID" )]
		public UUID GUID { get; set; }

		[Serialize("ObjectTypeID" )]
		public uint ObjectTypeID { get; set; }

		[Serialize("DateCreated" )]
		public DateTime DateCreated { get; set; }

		/// <summary>
		/// This property is used to Serialize NewMetadata relations
		/// </summary>
		[Serialize("Metadatas" )]
		public IEnumerable<Metadata> Metadatas { get; set; }

		/// <summary>
		/// This property is used to Serialize File relations
		/// </summary>
		[Serialize("Files" )]
		public IEnumerable<FileInfo> Files { get; set; }

		[Serialize("ObjectRelations" )]
		public List<Object_Object_Join> ObjectRealtions { get; set; }

        public IList<Link> Folders { get; set; }
		public IList<uint> FolderTree { get; set; }
		public List<Object> RelatedObjects { get; set; }
        public IList<AccessPoint_Object_Join> AccessPoints { get; set; }

        public KeyValuePair<string, string> UniqueIdentifier
        {
            get { return new KeyValuePair<string, string>( "GUID", GUID.ToString() ); }
        }

		#endregion
		#region Constructor

        public Object( Guid guid, uint objectTypeID, DateTime dateCreated, IEnumerable<Metadata> metadatas, IEnumerable<FileInfo> fileInfos, IEnumerable<Object_Object_Join> objectObjectJoins, IEnumerable<Link> folders, IEnumerable<AccessPoint_Object_Join> accessPoints ) 
		{
			GUID            = new UUID( guid.ToByteArray() );
			ObjectTypeID    = objectTypeID;
			DateCreated     = dateCreated;

			Metadatas       = metadatas.ToList();
			Files           = fileInfos.ToList();
			ObjectRealtions = objectObjectJoins.ToList();
            RelatedObjects  = new List<Object>();
			Folders         = folders.ToList();
            FolderTree      = new List<uint>();
            AccessPoints    = accessPoints.ToList();
            Fullname        = "Chaos.Mcm.Data.DTO.Object";
		}

		public Object() : this(Guid.Empty,uint.MinValue,DateTime.MinValue,new List<Metadata>(),new List<FileInfo>(),new List<Object_Object_Join>(),new List<Link>(),new List<AccessPoint_Object_Join>() )
		{
			
		}

		#endregion
		#region Business Logic

		public IEnumerable<KeyValuePair<string, string>> GetIndexableFields( )
		{
			yield return new KeyValuePair<string, string>("GUID", GUID.ToString( ) );
			yield return new KeyValuePair<string, string>("ObjectTypeID", ObjectTypeID.ToString(CultureInfo.InvariantCulture) );
			yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );

			if( Folders != null )
				foreach( var folder in Folders )
				{
					yield return new KeyValuePair<string, string>("FolderID", folder.FolderID.ToString(CultureInfo.InvariantCulture) );
				}

			if( FolderTree != null )
				foreach( var folderID in FolderTree )
				{
					yield return new KeyValuePair<string, string>("FolderTree", folderID.ToString(CultureInfo.InvariantCulture) );
				}

			// TODO: Implement NewMetadata XML converter

			// Convert to all field
			if( Metadatas != null )
				foreach( var metadata in Metadatas )
				{
					switch( metadata.MetadataSchemaGUID.ToString() )
					{
						case "e4ee26e4-94dc-d946-8e23-459c7de51fc0":
							if( metadata.MetadataXML.Descendants("TotalVotes").FirstOrDefault() != null )
								yield return new KeyValuePair<string, string>( "LB_TotalVotes", metadata.MetadataXML.Descendants( "TotalVotes" ).First().Value );
							break;
						case "f39ac380-e33d-7c4e-9ed9-7745990ed6c7":
							if( metadata.MetadataXML.Descendants( "TotalVotes" ).FirstOrDefault() != null )
								yield return new KeyValuePair<string, string>( "HT_TotalVotes", metadata.MetadataXML.Descendants( "TotalVotes" ).First().Value );
							break;
						case "21453740-eb1a-8842-81b4-ec62975e89e0":
							if( metadata.MetadataXML.Descendants("Country").FirstOrDefault() != null )
								yield return new KeyValuePair<string, string>( "HT_Country_" + metadata.LanguageCode, metadata.MetadataXML.Descendants( "Country" ).First().Value );
							break;
						case "d9efe8c8-9502-11e1-ba5d-02cea2621172":
							if( metadata.MetadataXML.Descendants("FacebookIds").FirstOrDefault() != null )
								foreach( var id in metadata.MetadataXML.Descendants("FacebookIds").Elements( "Id" ) )
									yield return new KeyValuePair<string, string>( "FacebookUserIDs", id.Value );
							break;
						// DKA2
						case "5906a41b-feae-48db-bfb7-714b3e105396":
							var ns        = metadata.MetadataXML.Root.GetNamespaceOfPrefix( "dka" );
					        var defaultNs = metadata.MetadataXML.Root.GetDefaultNamespace();

							if( ns != null && metadata.MetadataXML.Descendants( XName.Get( "ExternalIdentifier", ns.NamespaceName ) ).FirstOrDefault() != null )
								yield return new KeyValuePair<string, string>( "DKA-ExternalIdentifier", metadata.MetadataXML.Descendants( XName.Get( "ExternalIdentifier", ns.NamespaceName ) ).First().Value );
                            else if( defaultNs != null && metadata.MetadataXML.Descendants( XName.Get( "ExternalIdentifier", defaultNs.NamespaceName ) ).FirstOrDefault() != null )
                                yield return new KeyValuePair<string, string>( "DKA-ExternalIdentifier", metadata.MetadataXML.Descendants( XName.Get( "ExternalIdentifier", defaultNs.NamespaceName ) ).First().Value );
                            else if (metadata.MetadataXML.Descendants("ExternalIdentifier").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>( "DKA-ExternalIdentifier", metadata.MetadataXML.Descendants( "ExternalIdentifier" ).First().Value );
							break;
						// DKA
						// TODO: Remember to add namespace to DKA fields when DKA is replaced by DKA2
						case "00000000-0000-0000-0000-000063c30000":
							if( metadata.MetadataXML.Descendants("Organization").FirstOrDefault() != null )
								yield return new KeyValuePair<string, string>( "DKA-Organization", metadata.MetadataXML.Descendants("Organization").First().Value );

							if( metadata.MetadataXML.Root.Element("Type") != null )
								yield return new KeyValuePair<string, string>( "DKA-Type", metadata.MetadataXML.Root.Element("Type").Value );
							break;
						case "d361328e-4fd2-4cb1-a2b4-37ecc7679a6e":
							if( metadata.MetadataXML.Descendants("ID").FirstOrDefault() != null )
								yield return new KeyValuePair<string, string>( "DKA-DFI-ID", metadata.MetadataXML.Root.Element("ID").Value );
							break;
						case "1fd4e56e-3f3a-4f25-ba3e-3d9f80d5d49e":
							if( metadata.MetadataXML.Root.Element("Name") != null )
								yield return new KeyValuePair<string, string>( "CHAOS-Profile-Name", metadata.MetadataXML.Root.Element("Name").Value );
							break;

                        // FIAT / IFTA
                        case "22c70550-90ce-43f9-9176-973c09760138":
                            if (!string.IsNullOrEmpty(metadata.MetadataXML.Root.Element("FirstPublicationDate").Value))
                                yield return new KeyValuePair<string, string>("FIATIFTA-ANP-FirstPublicationDate", 
                                    DateTime.Parse(metadata.MetadataXML.Root.Element("FirstPublicationDate").Value).ToString( "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" )  );

                            if (!string.IsNullOrEmpty(metadata.MetadataXML.Root.Element("Title").Value))
                                yield return new KeyValuePair<string, string>("FIATIFTA-ANP-Title", metadata.MetadataXML.Root.Element("Title").Value);

                            if (!string.IsNullOrEmpty(metadata.MetadataXML.Root.Element("Publisher").Value))
                                yield return new KeyValuePair<string, string>("FIATIFTA-ANP-Publisher", metadata.MetadataXML.Root.Element("Publisher").Value);
                            break;

						// LARM Program
						case "00000000-0000-0000-0000-0000df820000":
							if( metadata.MetadataXML.Root.Element("PublicationDateTime") != null && metadata.MetadataXML.Root.Element("PublicationEndDateTime") != null )
							{
								DateTime larmPubStartDate;
								DateTime larmPubEndDate;

								var dateTimeFormat1 = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
								var dateTimeFormat2 = "dd'-'MM'-'yyyy HH':'mm':'ss";
								var dateTimeFormat3 = "dd'/'MM'/'yyyy HH':'mm':'ss";
								var dateTimeFormat4 = "yyyy'-'MM'-'dd HH':'mm':'ss";
                                var dateTimeFormat5 = "yyyy-MM-ddTHH:mm:ss.fffzzz";

								var formatProvider = System.Globalization.CultureInfo.InvariantCulture;
								var larmPubStartDateString = metadata.MetadataXML.Root.Element("PublicationDateTime").Value;
								var larmPubEndDateString   = metadata.MetadataXML.Root.Element("PublicationEndDateTime").Value;

								if( !DateTime.TryParseExact( larmPubStartDateString, dateTimeFormat1, formatProvider, DateTimeStyles.None, out larmPubStartDate ) &&
								    !DateTime.TryParseExact( larmPubStartDateString, dateTimeFormat2, formatProvider, DateTimeStyles.None, out larmPubStartDate ) &&
									!DateTime.TryParseExact( larmPubStartDateString, dateTimeFormat3, formatProvider, DateTimeStyles.None, out larmPubStartDate ) &&
									!DateTime.TryParseExact( larmPubStartDateString, dateTimeFormat4, formatProvider, DateTimeStyles.None, out larmPubStartDate ) &&
                                    !DateTime.TryParseExact( larmPubStartDateString, dateTimeFormat5, formatProvider, DateTimeStyles.None, out larmPubStartDate ))
									break;

								if( !DateTime.TryParseExact( larmPubEndDateString, dateTimeFormat1, formatProvider, DateTimeStyles.None, out larmPubEndDate ) &&
								    !DateTime.TryParseExact( larmPubEndDateString, dateTimeFormat2, formatProvider, DateTimeStyles.None, out larmPubEndDate ) &&
									!DateTime.TryParseExact( larmPubEndDateString, dateTimeFormat3, formatProvider, DateTimeStyles.None, out larmPubEndDate ) &&
									!DateTime.TryParseExact( larmPubEndDateString, dateTimeFormat4, formatProvider, DateTimeStyles.None, out larmPubEndDate ) &&
                                    !DateTime.TryParseExact( larmPubEndDateString, dateTimeFormat5, formatProvider, DateTimeStyles.None, out larmPubEndDate ))
									break;

								yield return new KeyValuePair<string, string>( "LARM-PubStartDate", larmPubStartDate.ToString( "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", formatProvider ) );
								yield return new KeyValuePair<string, string>( "LARM-PubEndDate", larmPubEndDate.ToString( "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", formatProvider ) );
								
								if( larmPubEndDate.CompareTo( larmPubStartDate ) > 0 )
									yield return new KeyValuePair<string, string>( "LARM-Duration",  ( (uint) larmPubEndDate.Subtract( larmPubStartDate ).TotalSeconds ).ToString());
								else
									yield return new KeyValuePair<string, string>( "LARM-Duration",  ( (uint) larmPubStartDate.Subtract( larmPubEndDate ).TotalSeconds ).ToString());	

                                yield return new KeyValuePair<string, string>("LARM-Annotation-Count", RelatedObjects.Count(item => item.ObjectTypeID == 41 || item.ObjectTypeID == 64).ToString());

                                foreach (var obj in RelatedObjects)
                                {
                                    foreach (var relatedMetadata in obj.Metadatas)
                                    {
                                        yield return new KeyValuePair<string, string>(string.Format("rm{0}_{1}_all", relatedMetadata.MetadataSchemaGUID, relatedMetadata.LanguageCode), GetXmlContent(relatedMetadata.MetadataXML.Root));
                                    }
                                }
							}
							
							if( metadata.MetadataXML.Root.Element("PublicationChannel") != null )
								yield return new KeyValuePair<string, string>( "LARM-Channel", metadata.MetadataXML.Root.Element("PublicationChannel").Value );

							if( metadata.MetadataXML.Root.Element("Title") != null )
								yield return new KeyValuePair<string, string>( "LARM-Title", metadata.MetadataXML.Root.Element("Title").Value );
							break;
						case "70c26faf-b1ee-41e8-b916-a5a16b25ca69":
							if( metadata.MetadataXML.Root.Element("Date") != null )
								yield return new KeyValuePair<string, string>( "LARM-PubStartDate", DateTime.Parse(metadata.MetadataXML.Root.Element("Date").Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );
							if( metadata.MetadataXML.Root.Element("Title") != null )
								yield return new KeyValuePair<string, string>( "LARM-Title", metadata.MetadataXML.Root.Element("Title").Value );
							break;
						case "c82a6f6d-b56b-4662-9627-f19410afc309":
							if( metadata.MetadataXML.Root.Element("Keywords") != null )
								foreach( var keyword in metadata.MetadataXML.Root.Element("Keywords").Value.Split(' ') )
								{
									yield return new KeyValuePair<string,string>( "LARM-Test2-Keyword", keyword.Replace( "%20", " " ) );
								}
							break;
                        case "bb615cd5-4470-ce4a-9207-b18e8ae33880":
                            if (metadata.MetadataXML.Root.Element("sequenceIndex") != null)
                                yield return new KeyValuePair<string, string>("KN-SequenceIndex", metadata.MetadataXML.Root.Element("sequenceIndex").Value);
                            break;

					}

					yield return new KeyValuePair<string, string>( string.Format( "m{0}_{1}_all", metadata.MetadataSchemaGUID, metadata.LanguageCode ), GetXmlContent( metadata.MetadataXML.Root ) );
				}

            if (RelatedObjects != null)
            {
                foreach (var obj in RelatedObjects)
                {
                    foreach (var relatedMetadata in obj.Metadatas)
                    {
                        yield return new KeyValuePair<string, string>(string.Format("rm{0}_{1}_all", relatedMetadata.MetadataSchemaGUID, relatedMetadata.LanguageCode), GetXmlContent(relatedMetadata.MetadataXML.Root));
                    }
                }
            }

			//if( AccessPoints != null )
			//	foreach( var accessPoint in AccessPoints )
			//	{
			//		if( accessPoint.StartDate.HasValue )
			//			yield return new KeyValuePair<string, string>( "PubStart", accessPoint.StartDate.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );
			//		else 
			//			yield return new KeyValuePair<string, string>( "PubStart", DateTime.MaxValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );
                    
			//		if( accessPoint.EndDate.HasValue )
			//			yield return new KeyValuePair<string, string>( "PubEnd", accessPoint.EndDate.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );
			//		else
			//			yield return new KeyValuePair<string, string>( "PubEnd", DateTime.MaxValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );

			//		break;
			//	}

			// TODO: Modify accessPoint logic to support multiple accessPoints with different publish settings
			var accessPoint = AccessPoints.FirstOrDefault();

			if( accessPoint != null && accessPoint.StartDate.HasValue )
                yield return new KeyValuePair<string, string>( "PubStart", accessPoint.StartDate.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );
                    
            if( accessPoint != null && accessPoint.EndDate.HasValue )
                yield return new KeyValuePair<string, string>( "PubEnd", accessPoint.EndDate.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'" ) );
		}

	    private static string GetXmlContent( XContainer xml )
		{
			var sb = new StringBuilder( );

			foreach( var node in xml.Descendants( ) )
			{
				if( !node.HasElements )
					sb.AppendLine(node.Value );
			}

			return sb.ToString( );
		}

		#endregion
	}
}
