﻿namespace Chaos.Mcm.Test.View
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Chaos.Mcm.Data.Dto;
    using Chaos.Mcm.Permission.InMemory;
    using Chaos.Mcm.View;

    using Moq;

    using NUnit.Framework;

    using FileInfo = Chaos.Mcm.Data.Dto.FileInfo;

    [TestFixture]
    public class ObjectIndexTest : Test.TestBase
    {
        [SetUp]
        public new void SetUp()
        {
            var folder = new Folder();
            PermissionManager.Setup(m => m.GetFolders(It.IsAny<uint>())).Returns(folder);
        }

        [Test]
        public void Index_GivenObject_ReturnObjectViewData()
        {
            var view = Make_ObjectView();
            var obj  = Make_Object();

            var result = (ObjectViewData) view.Index(obj).First();

            Assert.That(result.Id, Is.EqualTo(obj.Guid.ToString()));
            Assert.That(result.ObjectTypeId, Is.EqualTo(obj.ObjectTypeID));
            Assert.That(result.DateCreated, Is.EqualTo(obj.DateCreated));
            Assert.That(result.AccessPoints, Is.SameAs(obj.AccessPoints));
//            Assert.That(result.Files, Is.SameAs(obj.Files));
            Assert.That(result.Metadatas, Is.SameAs(obj.Metadatas));
            Assert.That(result.ObjectFolders, Is.SameAs(obj.ObjectFolders));
//            Assert.That(result.ObjectRelationInfos, Is.SameAs(obj.ObjectRelationInfos));
        }

        private ObjectView Make_ObjectView()
        {
            return new ObjectView(PermissionManager.Object);
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithAccessPoints_ReturnPublishStartAndEndDates()
        {
            var obj = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.Any(item => item.Key.Contains(obj.AccessPoints.First().AccessPointGuid + "_PubStart")), Is.True);
            Assert.That(results.Any(item => item.Key.Contains(obj.AccessPoints.First().AccessPointGuid + "_PubEnd")), Is.True);
        }

        [Test]
        public void GetIndexableFields_GivenObject_ReturnObjectTypeId()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.First(item => item.Key == "ObjectTypeId").Value, Is.EqualTo("1"));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithFiles_ReturnFileCount()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.First(item => item.Key == "Files.Count").Value, Is.EqualTo("1"));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithObjectRelations_ReturnRelationCount()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.First(item => item.Key == "ObjectRelations.Count").Value, Is.EqualTo("1"));
        }

        [Test]
        public void GetIndexableFields_GivenObjectWithFolders_ReturnFolders()
        {
            var obj  = Make_Object();
            var data = new ObjectViewData(obj, PermissionManager.Object);

            var results = data.GetIndexableFields().ToList();

            Assert.That(results.Any(item => item.Key.Contains("FolderId")), Is.True);
            Assert.That(results.Any(item => item.Key.Contains("FolderAncestors")), Is.True);
        }
        
        private static Object Make_Object()
        {
            return new Object{
                    ObjectTypeID = 1,
                    AccessPoints = new []
                        {
                            new ObjectAccessPoint
                                {
                                    AccessPointGuid = new System.Guid("00000000-0000-0000-0000-000000000001")
                                }
                        },
                        ObjectFolders = new []
                            {
                                new ObjectFolder
                                    {
                                        ID = 10
                                    }
                            },
                            Files = new List<FileInfo>()
                                {
                                    new FileInfo()
                                },
                                ObjectRelationInfos = new List<ObjectRelationInfo>()
                                    {
                                        new ObjectRelationInfo()
                                    }
                };
        }
    }
}