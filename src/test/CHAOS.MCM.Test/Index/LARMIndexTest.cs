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
    public class LARMIndexTest
    {
        [TestMethod]
        public void Should_Index_LARM_Radio_Udsendelse_Tags()
        {
            var indexableFieldsList = CreateTestObject(
                GetLarmRadioUdsendelseMetadata(), "7e08dbc3-c60c-4b42-bcd8-8d0ed8dbba36", 0).GetIndexableFields();

            Assert.AreEqual(indexableFieldsList.First(a => a.Key == "LARM-radio-udsendelse_tags_stringmv").Value, "første tag");
        }


        private Data.Dto.Standard.Object CreateTestObject(string metadataXml, string schema, uint objectTypeId)
        {
            var objectGuid = new UUID("a2cdc9c3-f16d-4845-b8fe-321071c10cda").ToGuid();

            var larmProgramMetadata = new Metadata(Guid.NewGuid(), objectGuid, "da",
                                                                 new UUID(schema).ToGuid(), 1,
                                                                 metadataXml, DateTime.Now, Guid.NewGuid());

            var metadataList = new List<Metadata> { larmProgramMetadata };

            var fileInfoList = new List<FileInfo>();

            var ObjectObjectJoinList = new List<Object_Object_Join>();

            var linkList = new List<Link>();

            var accessPointObjectList = new List<AccessPoint_Object_Join>();

            return new Data.Dto.Standard.Object(objectGuid, objectTypeId, DateTime.Now, metadataList, fileInfoList, ObjectObjectJoinList, linkList,
                                                         accessPointObjectList);

        }

        private string GetLarmRadioUdsendelseMetadata()
        {
            return "<metadata><titel>Trappeskakt</titel> <varighed></varighed> <serietitel></serietitel> <producenter><producent>Jeppe Greve</producent></producenter><beskrivelse>null</beskrivelse><tags><tag>første tag</tag><tag>Andet tag</tag></tags></metadata>";
        }
    }
}
