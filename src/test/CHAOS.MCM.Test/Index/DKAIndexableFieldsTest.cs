using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CHAOS.Extensions;
using CHAOS.MCM.Data.Dto.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CHAOS.MCM.Test.Index
{
    [TestClass]
    public class DKAIndexableFieldsTest
    {
        private Dictionary<string, string> metadataDictionary = new Dictionary<string, string>();


        [TestInitialize]
        public void Setup()
        {
            metadataDictionary.Add("DKA2_0", GetDKA2_0());
            metadataDictionary.Add("CrowdTag_0", GetCrowdTag_0());
            metadataDictionary.Add("Crowd_0", GetCrowd_0());
            metadataDictionary.Add("Crowd_1", GetCrowd_1());
            metadataDictionary.Add("Collection_0", GetCollection_0());
            metadataDictionary.Add("Collection_1", GetCollection_1());

        }

        [TestMethod]
        public void Should_Get_DKA_Collection_IndexableFields()
        {
            var indexableFieldsList_0 =
                CreateTestObject("Collection_0", "00000000-0000-0000-0000-000065c30000", 10).GetIndexableFields();

            var indexableFieldsList_1 =
              CreateTestObject("Collection_1", "00000000-0000-0000-0000-000065c30000", 10).GetIndexableFields();
        }


        [TestMethod]
        public void Should_Get_DKA_Crowd_IndexableFields()
        {
            var indexableFieldsList = CreateTestObject("Crowd_0", "a37167e0-e13b-4d29-8a41-b0ffbaa1fe5f", 36).GetIndexableFields();

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Views_int").Value, "26");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Shares_int").Value, "0");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Likes_int").Value, "0");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Ratings_int").Value, "0");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Slug_string").Value, "sårede-sønderjyske-krigsfanger-på-feltlazaret-på-britisk-hospitalsskib");
        }

        [TestMethod]
        public void Should_Get_DKA_Crowd_IndexableFields_1()
        {
            var indexableFieldsList = CreateTestObject("Crowd_1", "a37167e0-e13b-4d29-8a41-b0ffbaa1fe5f", 36).GetIndexableFields();

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Views_int").Value, "3");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Shares_int").Value, "0");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Likes_int").Value, "0");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Ratings_int").Value, "0");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Slug_string").Value, "materiale-uden-titel-791");
        }

        [TestMethod]
        public void Should_Get_DKA_Crowd_Tag_IndexableFields()
        {
            var indexableFieldsList = CreateTestObject("CrowdTag_0", "00000000-0000-0000-0000-000067c30000", 36).GetIndexableFields();

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Tag-Created_date").Value, "2013-11-18T13:34:38Z");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Tag-Status_string").Value, "Unapproved");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Crowd-Tag-Value_string").Value, "Tror det er fake. Armbindene er tyske, støvlerne ikke typiske for NSDAP!");
        }

        [TestMethod]
        public void Should_Get_DKA_Program_IndexableFields()
        {
            var indexableFieldsList = CreateTestObject("DKA2_0", "5906a41b-feae-48db-bfb7-714b3e105396", 36).GetIndexableFields();

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "GUID").Value, "a2cdc9c3-f16d-4845-b8fe-321071c10cda");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "ObjectTypeID").Value, "36");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-ExternalIdentifier").Value, "oai:kb.dk:oai:kb.dk:images:billed:2010:okt:billeder:object125597");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Organization").Value, "Det Kongelige Bibliotek");

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "DKA-Title_string").Value, "Lisbet Hindgaul sammen Knut Getz Wold");

            //Assert.Contains("Formanden for Foreningen Nordens", indexableFieldsList.First(a => a.Key == "m5906a41b-feae-48db-bfb7-714b3e105396_da_all").Value.ToList());

        }

        private Data.Dto.Standard.Object CreateTestObject(string metadataKey, string schema, uint objectTypeId)
        {
            var objectGuid = new UUID("a2cdc9c3-f16d-4845-b8fe-321071c10cda").ToGuid();

            var dka2ProgramMetadata = new Metadata(Guid.NewGuid(), objectGuid, "da",
                                                                 new UUID(schema).ToGuid(), 1,
                                                                 metadataDictionary[metadataKey], DateTime.Now, Guid.NewGuid());

            var metadataList = new List<Metadata> { dka2ProgramMetadata };

            var fileInfoList = new List<FileInfo>();

            var ObjectObjectJoinList = new List<Object_Object_Join>();

            var linkList = new List<Link>();

            var accessPointObjectList = new List<AccessPoint_Object_Join>();

            return new Data.Dto.Standard.Object(objectGuid, objectTypeId, DateTime.Now, metadataList, fileInfoList, ObjectObjectJoinList, linkList,
                                                         accessPointObjectList);

        }

        private string GetDKA2_0()
        {
            return @"<?xml version='1.0'?>
                        <DKA xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://www.danskkulturarv.dk/DKA2.xsd' xmlns:oa='http://www.openarchives.org/OAI/2.0/' xmlns:ese='http://www.europeana.eu/schemas/ese/' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:dcterms='http://purl.org/dc/terms/' xsi:schemaLocation='http://www.danskkulturarv.dk/DKA2.xsd ../../Base/schemas/DKA2.xsd'>
                            <Title>Lisbet Hindgaul sammen Knut Getz Wold</Title>
                            <Abstract/>
                            <Description><div xmlns='http://www.w3.org/1999/xhtml'><p>Formanden for Foreningen Nordens K&#xF8;benhavnsafdeling Lisbet Hindgaul og Centralbankchef Knut Getz Wold, som talte ved et frokostm&#xF8;de 4. april 1963 p&#xE5; Hotel Richmond om Norge og markedsproblemerne</p><p><strong><a target='_blank' href='http://www.kb.dk/images/billed/2010/okt/billeder/da/'>
			                Mere fra samme udgivelse</a></strong></p></div></Description>
                            <Organization>Det Kongelige Bibliotek</Organization>
                            <ExternalURL>http://www.kb.dk/images/billed/2010/okt/billeder/object125597/en/</ExternalURL>
                            <ExternalIdentifier>oai:kb.dk:oai:kb.dk:images:billed:2010:okt:billeder:object125597</ExternalIdentifier>
                            <Type>IMAGE</Type>
                            <Contributors/><Creators><Creator Role='creator' Name='Woldbye, Janne (f. 1923) fotograf'/></Creators><TechnicalComment/>
                            <Location>Danmark, K&#xF8;benhavn, Vester Farimagsgade 33, Hotel Richmond</Location>
                            <RightsDescription>Copyright &#xA9; The Royal Library: The National Library of Denmark and Copenhagen University Library</RightsDescription>
                            <Categories/>
                            <Tags><Tag>Foreningen Norden</Tag><Tag>Hindsgaul, Lisbet, f. Jonsen (1890-1969) politiker, statsrevisor</Tag><Tag>Wold, Knut Getz (1915-1987) cand. oecon., sentralbanksjef, Norges Bank</Tag></Tags>
                        </DKA>";

        }

        private string GetCrowdTag_0()
        {
            return @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
                            <dkact:Tag xmlns:dkact='http://www.danskkulturarv.dk/DKA-Crowd-Tag.xsd' created='2013-11-18T12:34:38+00:00' status='Unapproved'>Tror det er fake. Armbindene er tyske, støvlerne ikke typiske for NSDAP!</dkact:Tag>
                            ";
        }

        private string GetCrowd_0()
        {
            return @"<?xml version='1.0'?>
<dkac:DKACrowd xmlns:dkac='http://www.danskkulturarv.dk/DKA-Crowd.xsd'><dkac:Views>26</dkac:Views><dkac:Shares>0</dkac:Shares><dkac:Likes>0</dkac:Likes><dkac:Ratings>0</dkac:Ratings><dkac:AccumulatedRate>0</dkac:AccumulatedRate><dkac:Slug>s&#xE5;rede-s&#xF8;nderjyske-krigsfanger-p&#xE5;-feltlazaret-p&#xE5;-britisk-hospitalsskib</dkac:Slug></dkac:DKACrowd>
";

        }

        private string GetCrowd_1()
        {
            return @"<?xml version='1.0'?>
<dkac:DKACrowd xmlns:dkac='http://www.danskkulturarv.dk/DKA-Crowd.xsd'><dkac:Views>3</dkac:Views><dkac:Shares>0</dkac:Shares><dkac:Likes>0</dkac:Likes><dkac:Ratings>0</dkac:Ratings><dkac:AccumulatedRate>0</dkac:AccumulatedRate><dkac:Slug>materiale-uden-titel-791</dkac:Slug></dkac:DKACrowd>
";
        }

        private string GetCollection_0()
        {
            return @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
<dkac:Collection xmlns:dkac='http://www.danskkulturarv.dk/DKA-Collection.xsd'>
<dkac:Title>Title test</dkac:Title><dkac:Description>Description Test</dkac:Description><dkac:Rights>Right test</dkac:Rights><dkac:Type>Series</dkac:Type><dkac:Status>Draft</dkac:Status><dkac:Playlist/></dkac:Collection>";

        }

        private string GetCollection_1()
        {
            return @"<Collection><Title>Bikstok Røgsystem</Title><Description></Description><Category></Category><Rights>310</Rights></Collection>";
        }
    }
}
