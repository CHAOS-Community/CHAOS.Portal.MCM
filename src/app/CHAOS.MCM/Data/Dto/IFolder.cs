using System;

namespace Chaos.Mcm.Data.Dto
{
    public interface IFolder
    {
        uint ID { get; set; }
        uint? ParentID { get; set; }
        uint FolderTypeID { get; set; }
        Guid? SubscriptionGuid { get; set; }
        string Name { get; set; }
        DateTime DateCreated { get; set; }
    }
}