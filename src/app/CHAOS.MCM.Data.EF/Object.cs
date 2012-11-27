using System.Collections.Generic;

namespace CHAOS.MCM.Data.EF
{
	public partial class Object
	{
		#region Properties

		public IEnumerable<Metadata> pMetadatas { get; set; }
		public IEnumerable<FileInfo> pFiles { get; set; }
		public IEnumerable<Object_Object_Join> ObjectRealtions { get; set; }
		public IEnumerable<Object> RelatedObjects { get; set; }
        public IEnumerable<Object_Folder_Join> Folders { get; set; }
        public IEnumerable<Object_Folder_Join> FolderTree { get; set; }
        public IEnumerable<AccessPoint_Object_Join> AccessPoints { get; set; }

		#endregion
		#region Constructor

		public Object()
		{
			pMetadatas      = new List<Metadata>();
			pFiles          = new List<FileInfo>();
			ObjectRealtions = new List<Object_Object_Join>();
			RelatedObjects  = new List<Object>();
            Folders         = new List<Object_Folder_Join>();
            FolderTree      = new List<Object_Folder_Join>();
            AccessPoints    = new List<AccessPoint_Object_Join>();
		}

		#endregion
	}
}
