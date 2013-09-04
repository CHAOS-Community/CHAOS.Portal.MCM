namespace Chaos.Mcm.Test.View
{
    using System.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.View;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectIndexTest
    {
        [Test]
        public void Index_GivenObject_ReturnObjectViewData()
        {
            var view = new ObjectView();
            var obj  = Make_Object();

            var result = (ObjectViewData) view.Index(obj).First();

            Assert.That(result.Id, Is.EqualTo(obj.Guid.ToString()));
            Assert.That(result.ObjectTypeId, Is.EqualTo(obj.ObjectTypeID));
            Assert.That(result.DateCreated, Is.EqualTo(obj.DateCreated));
            Assert.That(result.AccessPoints, Is.SameAs(obj.AccessPoints));
            Assert.That(result.Files, Is.SameAs(obj.Files));
            Assert.That(result.Metadatas, Is.SameAs(obj.Metadatas));
            Assert.That(result.ObjectFolders, Is.SameAs(obj.ObjectFolders));
            Assert.That(result.ObjectRelationInfos, Is.SameAs(obj.ObjectRelationInfos));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithAccessPoints_ReturnPublishStartAndEndDates()
        {
            var obj = Make_Object();
            var data = new ObjectViewData(obj);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.Any(item => item.Key.Contains(obj.AccessPoints.First().AccessPointGuid + "_PubStart")), Is.True);
            Assert.That(results.Any(item => item.Key.Contains(obj.AccessPoints.First().AccessPointGuid + "_PubEnd")), Is.True);
        }

        private static Object Make_Object()
        {
            return new Object
                {
                    AccessPoints = new []
                        {
                            new ObjectAccessPoint
                                {
                                    AccessPointGuid = new System.Guid("00000000-0000-0000-0000-000000000001")
                                }
                        }
                };
        }
    }
}