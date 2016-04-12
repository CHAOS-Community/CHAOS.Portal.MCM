using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CHAOS.Index;
using CHAOS.Portal.DTO.Standard;
using CHAOS.Serialization;

namespace CHAOS.MCM.Data.Dto.Standard
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
		/// This property is used to Serialize Metadata relations
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

        [Serialize("AccessPoints")]
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
            Fullname        = "CHAOS.MCM.Data.DTO.Object";
		}

		public Object() : this(Guid.Empty,uint.MinValue,DateTime.MinValue,new List<Metadata>(),new List<FileInfo>(),new List<Object_Object_Join>(),new List<Link>(),new List<AccessPoint_Object_Join>() )
		{
			
		}

		#endregion
		#region Business Logic

        public IEnumerable<KeyValuePair<string, string>> GetIndexableFields()
        {
            yield return new KeyValuePair<string, string>("GUID", GUID.ToString());
            yield return new KeyValuePair<string, string>("ObjectTypeID", ObjectTypeID.ToString(CultureInfo.InvariantCulture));
            yield return new KeyValuePair<string, string>("DateCreated", DateCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));

            if (Folders != null)
                foreach (var folder in Folders)
                {
                    yield return new KeyValuePair<string, string>("FolderID", folder.FolderID.ToString(CultureInfo.InvariantCulture));
                }

            if (FolderTree != null)
                foreach (var folderID in FolderTree)
                {
                    yield return new KeyValuePair<string, string>("FolderTree", folderID.ToString(CultureInfo.InvariantCulture));
                }

            if (Files != null)
            {
                foreach (var file in Files)
                {
                    yield return new KeyValuePair<string, string>("FormatTypeName", file.FormatTypeName);
                }
            }

            // TODO: Implement Metadata XML converter

            // Convert to all field
            if (Metadatas != null)
                foreach (var metadata in Metadatas)
                {
                    switch (metadata.MetadataSchemaGUID.ToString())
                    {
                        #region Content
                        case "e4ee26e4-94dc-d946-8e23-459c7de51fc0":
                            if (metadata.MetadataXML.Descendants("TotalVotes").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("LB_TotalVotes", metadata.MetadataXML.Descendants("TotalVotes").First().Value);
                            break;
                        case "f39ac380-e33d-7c4e-9ed9-7745990ed6c7":
                            if (metadata.MetadataXML.Descendants("TotalVotes").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("HT_TotalVotes", metadata.MetadataXML.Descendants("TotalVotes").First().Value);
                            break;
                        case "21453740-eb1a-8842-81b4-ec62975e89e0":
                            if (metadata.MetadataXML.Descendants("Country").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("HT_Country_" + metadata.LanguageCode, metadata.MetadataXML.Descendants("Country").First().Value);
                            break;
                        case "d9efe8c8-9502-11e1-ba5d-02cea2621172":
                            if (metadata.MetadataXML.Descendants("FacebookIds").FirstOrDefault() != null)
                                foreach (var id in metadata.MetadataXML.Descendants("FacebookIds").Elements("Id"))
                                    yield return new KeyValuePair<string, string>("FacebookUserIDs", id.Value);
                            break;
                        #endregion
                        #region DKA
                        DateTime dtFirstPublishedDate;


                        #region DKA Tag
                        case "00000000-0000-0000-0000-000067c30000":
                            XNamespace nsDKATag = "http://www.danskkulturarv.dk/DKA-Crowd-Tag.xsd";

                            var dtTagCreated = DateTime.MinValue;

                            if (metadata.MetadataXML.Root.Attribute("created") != null)
                                if (DateTime.TryParse(metadata.MetadataXML.Root.Attribute("created").Value, out dtTagCreated))
                                    yield return new KeyValuePair<string, string>("DKA-Crowd-Tag-Created_date", dtTagCreated.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));

                            if (metadata.MetadataXML.Root.Attribute("status") != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Tag-Status_string", metadata.MetadataXML.Root.Attribute("status").Value);

                            if(metadata.MetadataXML.Descendants(nsDKATag + "Tag").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Tag-Value_string", metadata.MetadataXML.Descendants(nsDKATag + "Tag").First().Value);

                            break;
                        #endregion

                        #region DKA Crowd
                        case "a37167e0-e13b-4d29-8a41-b0ffbaa1fe5f":
                            XNamespace nsDKAC = "http://www.danskkulturarv.dk/DKA-Crowd.xsd";

                            // Views
                            if (metadata.MetadataXML.Descendants(nsDKAC + "Views").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Views_int", metadata.MetadataXML.Descendants(nsDKAC + "Views").First().Value);
                            else
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Views_int", "0");

                            // Shares
                            if (metadata.MetadataXML.Descendants(nsDKAC + "Shares").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Shares_int", metadata.MetadataXML.Descendants(nsDKAC + "Shares").First().Value);
                            else
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Shares_int", "0");

                            // Likes
                            if (metadata.MetadataXML.Descendants(nsDKAC + "Likes").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Likes_int", metadata.MetadataXML.Descendants(nsDKAC + "Likes").First().Value);
                            else
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Likes_int", "0");

                            // Ratings
                            if (metadata.MetadataXML.Descendants(nsDKAC + "Ratings").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Ratings_int", metadata.MetadataXML.Descendants(nsDKAC + "Ratings").First().Value);
                            else
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Ratings_int", "0");

                            // AccumulatedRate
                            if (metadata.MetadataXML.Descendants(nsDKAC + "AccumulatedRate").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-AccumulatedRate_int", metadata.MetadataXML.Descendants(nsDKAC + "AccumulatedRate").First().Value);
                            else
                                yield return new KeyValuePair<string, string>("DKA-Crowd-AccumulatedRate_int", "0");

                            // Slug
                            if (metadata.MetadataXML.Descendants(nsDKAC + "Slug").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-Slug_string", metadata.MetadataXML.Descendants(nsDKAC + "Slug").First().Value);

                            // ShortSlug
                            if (metadata.MetadataXML.Descendants(nsDKAC + "ShortSlug").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Crowd-ShortSlug_string", metadata.MetadataXML.Descendants(nsDKAC + "ShortSlug").First().Value);

                            // Tags
                            if (metadata.MetadataXML.Descendants(nsDKAC + "Tags").FirstOrDefault() != null)
                                if (metadata.MetadataXML.Descendants(nsDKAC + "Tag").Any())
                                {
                                    foreach (var tagElement in metadata.MetadataXML.Descendants(nsDKAC + "Tag"))
                                    {
                                        yield return new KeyValuePair<string, string>("DKA-Crowd-Tags_stringmv", tagElement.Value);
                                    }
                                }

                            break;
                        #endregion

                        #region DKA2
                        case "5906a41b-feae-48db-bfb7-714b3e105396":

                            var ns = metadata.MetadataXML.Root.GetNamespaceOfPrefix("dka");
                            var defaultNs = metadata.MetadataXML.Root.GetDefaultNamespace();

                            if (ns != null && metadata.MetadataXML.Descendants(XName.Get("ExternalIdentifier", ns.NamespaceName)).FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-ExternalIdentifier", metadata.MetadataXML.Descendants(XName.Get("ExternalIdentifier", ns.NamespaceName)).First().Value);
                            else if (defaultNs != null && metadata.MetadataXML.Descendants(XName.Get("ExternalIdentifier", defaultNs.NamespaceName)).FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-ExternalIdentifier", metadata.MetadataXML.Descendants(XName.Get("ExternalIdentifier", defaultNs.NamespaceName)).First().Value);
                            else if (metadata.MetadataXML.Descendants("ExternalIdentifier").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-ExternalIdentifier", metadata.MetadataXML.Descendants("ExternalIdentifier").First().Value);

                            //Organization
                            if (metadata.MetadataXML.Descendants(defaultNs +"Organization").FirstOrDefault() != null)
                                yield return
                                    new KeyValuePair<string, string>("DKA-Organization", metadata.MetadataXML.Descendants(defaultNs + "Organization").First().Value);
                            else
                                yield return
                                    new KeyValuePair<string, string>("DKA-Organization",
                                                                     GetDKAFallbackField(Metadatas, "Organization"));

                            //Title
                            if(metadata.MetadataXML.Descendants(defaultNs + "Title").FirstOrDefault() != null)
                                yield return 
                                    new KeyValuePair<string, string>("DKA-Title_string", metadata.MetadataXML.Descendants(defaultNs + "Title").First().Value);
                            else
                                yield return 
                                    new KeyValuePair<string, string>("DKA-Title_string", GetDKAFallbackField(Metadatas, "Title"));

                            //FirstPublishedDate
                            if (metadata.MetadataXML.Descendants(defaultNs + "FirstPublishedDate").FirstOrDefault() !=
                                null)
                            {
                                if (
                                    DateTime.TryParse(
                                        metadata.MetadataXML.Descendants(defaultNs + "FirstPublishedDate").First().Value,
                                        out dtFirstPublishedDate))
                                    yield return
                                        new KeyValuePair<string, string>("DKA-FirstPublishedDate_date",
                                                                         DateTime.Parse(
                                                                             metadata.MetadataXML.Descendants(
                                                                                 defaultNs + "FirstPublishedDate")
                                                                                     .First()
                                                                                     .Value)
                                                                                 .ToString(
                                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                                                                                     CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                if (DateTime.TryParse(GetDKAFallbackField(Metadatas, "FirstPublishedDate"),
                                                      out dtFirstPublishedDate))
                                    yield return
                                        new KeyValuePair<string, string>("DKA-FirstPublishedDate_date",
                                                                         DateTime.Parse(GetDKAFallbackField(Metadatas,
                                                                                                            "FirstPublishedDate"))
                                                                                 .ToString(
                                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                                                                                     CultureInfo.InvariantCulture));
                            }

                            break;

                        #endregion

                        #region DKA Collection
                        case "00000000-0000-0000-0000-000065c30000":
                            XNamespace nsDKACollection = "http://www.danskkulturarv.dk/DKA-Collection.xsd";

                            if (metadata.MetadataXML.Descendants(nsDKACollection + "Title").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Collection-Title_string", metadata.MetadataXML.Descendants(nsDKACollection + "Title").First().Value);

                            if (metadata.MetadataXML.Descendants(nsDKACollection + "Type").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Collection-Type_string", metadata.MetadataXML.Descendants(nsDKACollection + "Type").First().Value);

                            if (metadata.MetadataXML.Descendants(nsDKACollection + "Status").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-Collection-Status_string", metadata.MetadataXML.Descendants(nsDKACollection + "Status").First().Value);

                            break;

                        #endregion

                        // DKA
                        // TODO: Remember to add namespace to DKA fields when DKA is replaced by DKA2
                        case "00000000-0000-0000-0000-000063c30000":

                            if (metadata.MetadataXML.Descendants("Organization").FirstOrDefault() != null && !DoDKA2Exist(Metadatas))
                                yield return new KeyValuePair<string, string>("DKA-Organization", metadata.MetadataXML.Descendants("Organization").First().Value);

                           //if (metadata.MetadataXML.Descendants("Title").FirstOrDefault() != null && !DoDKA2Exist(Metadatas))
                           //     yield return new KeyValuePair<string, string>("DKA-Title_string", metadata.MetadataXML.Descendants("Title").First().Value);

                            //if (metadata.MetadataXML.Descendants("FirstPublishedDate").FirstOrDefault() != null && !DoDKA2Exist(Metadatas))
                              //  if (DateTime.TryParse(metadata.MetadataXML.Descendants("FirstPublishedDate").First().Value, out dtFirstPublishedDate))
                                //    yield return new KeyValuePair<string, string>("DKA-FirstPublishedDate_date", DateTime.Parse(metadata.MetadataXML.Descendants("FirstPublishedDate").First().Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture));

                            if (metadata.MetadataXML.Root.Element("Type") != null)
                                yield return new KeyValuePair<string, string>("DKA-Type", metadata.MetadataXML.Root.Element("Type").Value);
                            break;

                        //DKA DFI
                        case "d361328e-4fd2-4cb1-a2b4-37ecc7679a6e":
                            if (metadata.MetadataXML.Descendants("ID").FirstOrDefault() != null)
                                yield return new KeyValuePair<string, string>("DKA-DFI-ID", metadata.MetadataXML.Root.Element("ID").Value);
                            break;

                        case "1fd4e56e-3f3a-4f25-ba3e-3d9f80d5d49e":
                            if (metadata.MetadataXML.Root.Element("Name") != null)
                                yield return new KeyValuePair<string, string>("CHAOS-Profile-Name", metadata.MetadataXML.Root.Element("Name").Value);
                            break;
                        #endregion
                        #region FIAT / IFTA
                        // FIAT / IFTA
                        case "22c70550-90ce-43f9-9176-973c09760138":
                            if (!string.IsNullOrEmpty(metadata.MetadataXML.Root.Element("FirstPublicationDate").Value))
                                yield return new KeyValuePair<string, string>("FIATIFTA-ANP-FirstPublicationDate",
                                    DateTime.Parse(metadata.MetadataXML.Root.Element("FirstPublicationDate").Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));

                            if (!string.IsNullOrEmpty(metadata.MetadataXML.Root.Element("Title").Value))
                                yield return new KeyValuePair<string, string>("FIATIFTA-ANP-Title", metadata.MetadataXML.Root.Element("Title").Value);

                            if (!string.IsNullOrEmpty(metadata.MetadataXML.Root.Element("Publisher").Value))
                                yield return new KeyValuePair<string, string>("FIATIFTA-ANP-Publisher", metadata.MetadataXML.Root.Element("Publisher").Value);
                            break;
                        #endregion
                        #region LARM
                        #region LARM Program
                        case "00000000-0000-0000-0000-0000df820000":
                            if (metadata.MetadataXML.Root.Element("PublicationDateTime") != null && metadata.MetadataXML.Root.Element("PublicationEndDateTime") != null)
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
                                var larmPubEndDateString = metadata.MetadataXML.Root.Element("PublicationEndDateTime").Value;

                                if (!DateTime.TryParseExact(larmPubStartDateString, dateTimeFormat1, formatProvider, DateTimeStyles.None, out larmPubStartDate) &&
                                    !DateTime.TryParseExact(larmPubStartDateString, dateTimeFormat2, formatProvider, DateTimeStyles.None, out larmPubStartDate) &&
                                    !DateTime.TryParseExact(larmPubStartDateString, dateTimeFormat3, formatProvider, DateTimeStyles.None, out larmPubStartDate) &&
                                    !DateTime.TryParseExact(larmPubStartDateString, dateTimeFormat4, formatProvider, DateTimeStyles.None, out larmPubStartDate) &&
                                    !DateTime.TryParseExact(larmPubStartDateString, dateTimeFormat5, formatProvider, DateTimeStyles.None, out larmPubStartDate))
                                    break;

                                if (!DateTime.TryParseExact(larmPubEndDateString, dateTimeFormat1, formatProvider, DateTimeStyles.None, out larmPubEndDate) &&
                                    !DateTime.TryParseExact(larmPubEndDateString, dateTimeFormat2, formatProvider, DateTimeStyles.None, out larmPubEndDate) &&
                                    !DateTime.TryParseExact(larmPubEndDateString, dateTimeFormat3, formatProvider, DateTimeStyles.None, out larmPubEndDate) &&
                                    !DateTime.TryParseExact(larmPubEndDateString, dateTimeFormat4, formatProvider, DateTimeStyles.None, out larmPubEndDate) &&
                                    !DateTime.TryParseExact(larmPubEndDateString, dateTimeFormat5, formatProvider, DateTimeStyles.None, out larmPubEndDate))
                                    break;

                                yield return new KeyValuePair<string, string>("LARM-PubStartDate", larmPubStartDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", formatProvider));
                                yield return new KeyValuePair<string, string>("LARM-PubEndDate", larmPubEndDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", formatProvider));

                                if (larmPubEndDate.CompareTo(larmPubStartDate) > 0)
                                    yield return new KeyValuePair<string, string>("LARM-Duration", ((uint)larmPubEndDate.Subtract(larmPubStartDate).TotalSeconds).ToString());
                                else
                                    yield return new KeyValuePair<string, string>("LARM-Duration", ((uint)larmPubStartDate.Subtract(larmPubEndDate).TotalSeconds).ToString());

                                yield return new KeyValuePair<string, string>("LARM-Annotation-Count", RelatedObjects.Count(item => item.ObjectTypeID == 41 || item.ObjectTypeID == 64).ToString());

                            }
                            

                            if (RelatedObjects != null)
                            {
                                //Has related Annotation object
                                if(RelatedObjects.Any(obj => obj.ObjectTypeID == 64))
                                    yield return new KeyValuePair<string, string>("LARM-Program-Contain", "Annotation");

                                //Has related attached file object
                                if (RelatedObjects.Any(obj => obj.ObjectTypeID == 89))
                                    yield return new KeyValuePair<string, string>("LARM-Program-Contain", "attachedfile");
                            }


                            if (metadata.MetadataXML.Root.Element("PublicationChannel") != null)
                                yield return new KeyValuePair<string, string>("LARM-Channel", metadata.MetadataXML.Root.Element("PublicationChannel").Value);

                            if (metadata.MetadataXML.Root.Element("Title") != null)
                                yield return new KeyValuePair<string, string>("LARM-Title", metadata.MetadataXML.Root.Element("Title").Value.Trim().TrimStart('"').TrimEnd('"'));
                            break;
                        #endregion
                        #region LARM Programoversigter
                        case "70c26faf-b1ee-41e8-b916-a5a16b25ca69":
                            if (metadata.MetadataXML.Root.Element("Date") != null)
                                yield return new KeyValuePair<string, string>("LARM-PubStartDate", DateTime.Parse(metadata.MetadataXML.Root.Element("Date").Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                            if (metadata.MetadataXML.Root.Element("Title") != null)
                                yield return new KeyValuePair<string, string>("LARM-Title", metadata.MetadataXML.Root.Element("Title").Value);
                            break;

                        #endregion
                        #region Radioavis ocr rapporter
                        case "a3a39145-4b30-4732-80a1-fb9214c53654":
                            if (metadata.MetadataXML.Root.Element("ParsingValue") != null)
                                yield return new KeyValuePair<string, string>("LARM-OCR-Report_float", metadata.MetadataXML.Root.Element("ParsingValue").Value.Replace(',', '.'));

                       break;

                        #endregion
                        #region LARM Metadata
                        case "17d59e41-13fb-469a-a138-bb691f13f2ba":
                            if (metadata.MetadataXML.Root.Element("Tags") != null)
                                foreach (var keyword in metadata.MetadataXML.Root.Element("Tags").Value.Split(' '))
                                {
                                    yield return new KeyValuePair<string, string>("LARM-Metadata-Tags", keyword.Replace("%20", " "));
                                }

                            //Has LARM Metadata
                            if(metadata.MetadataXML.Root.Value != "")
                                yield return new KeyValuePair<string, string>("LARM-Program-Contain", "LARMMetadata");

                            break;
                        #endregion

                        case "7e08dbc3-c60c-4b42-bcd8-8d0ed8dbba36":
                            if (metadata.MetadataXML.Root.Element("tags") != null)
                            {
                                if (metadata.MetadataXML.Root.Element("tags").Elements("tag") != null)
                                {
                                    foreach (var tag in metadata.MetadataXML.Root.Element("tags").Elements("tag"))
                                    {
                                        yield return new KeyValuePair<string, string>("LARM-radio-udsendelse_tags_stringmv", tag.Value);
                                    }
                                }

                            }
                                

                        break;

                        #endregion
                        #region KulturarvNord
                        case "bb615cd5-4470-ce4a-9207-b18e8ae33880":
                            if (metadata.MetadataXML.Root.Element("sequenceIndex") != null)
                                yield return new KeyValuePair<string, string>("KN-SequenceIndex", metadata.MetadataXML.Root.Element("sequenceIndex").Value);
                            break;

                        case "614c6ca6-1bb6-4c46-a37f-778b3e978d7e":
                            if (metadata.MetadataXML.Root.Element("relationSequenceIndex") != null)
                                yield return new KeyValuePair<string, string>("KN-relationSequenceIndex", metadata.MetadataXML.Root.Element("relationSequenceIndex").Value);
                            break;

                        case "50fbc42d-981b-344c-8511-e84e21c930d6":
                            if (metadata.MetadataXML.Root.Element("arkivNr") != null)
                                yield return new KeyValuePair<string, string>("KN-arkivNr", metadata.MetadataXML.Root.Element("arkivNr").Value);
                            break;

                        #endregion
                    }

                    yield return new KeyValuePair<string, string>(string.Format("m{0}_{1}_all", metadata.MetadataSchemaGUID, metadata.LanguageCode), GetXmlContent(metadata.MetadataXML.Root));
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

            if (AccessPoints != null)
            foreach (var ap in AccessPoints)
            {
                var start = ap.StartDate.HasValue ? ap.StartDate.Value : DateTime.MaxValue;
                var end   = ap.EndDate.HasValue ? ap.EndDate.Value : DateTime.MaxValue;

                yield return new KeyValuePair<string, string>(string.Format("ap{0}_PubStart", ap.AccessPointGUID), start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                yield return new KeyValuePair<string, string>(string.Format("ap{0}_PubEnd", ap.AccessPointGUID), end.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
            }
        }

		private static string GetXmlContent2(XContainer xml)
		{
			return string.Join("\r\n", xml.Descendants().SelectMany(element => element.Nodes().OfType<XText>().Select(n => n.Value.Trim())));
		}

		private static bool DoDKA2Exist(IEnumerable<Metadata> metadatas)
        {
            return metadatas.Any(metadata => metadata.MetadataSchemaGUID.ToString().ToLower() == "5906a41b-feae-48db-bfb7-714b3e105396");
        }

	    private static string GetDKAFallbackField(IEnumerable<Metadata> metadatas, string fieldname)
        {
            foreach (var metadata in metadatas)
            {
                if (metadata.MetadataSchemaGUID.ToString().ToLower() == "00000000-0000-0000-0000-000063c30000")
                    if (metadata.MetadataXML.Descendants(fieldname).FirstOrDefault() != null)
                        return metadata.MetadataXML.Descendants(fieldname).First().Value;
 
            }
            return "";
        }

		#endregion
	}
}
