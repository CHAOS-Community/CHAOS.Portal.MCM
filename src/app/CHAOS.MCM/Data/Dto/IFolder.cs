using System;

namespace CHAOS.MCM.Data.Dto
{
    public interface IFolder
    {
        uint ID { get; set; }
        uint? ParentID { get; set; }
        uint FolderTypeID { get; set; }
        UUID SubscriptionGUID { get; set; }
        string Name { get; set; }
        DateTime DateCreated { get; set; }
    }
}