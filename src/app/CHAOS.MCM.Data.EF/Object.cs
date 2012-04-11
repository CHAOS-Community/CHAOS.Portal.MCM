﻿using System.Collections.Generic;

namespace CHAOS.MCM.Data.EF
{
	public partial class Object
	{
		#region Properties

		public IEnumerable<Metadata> pMetadatas { get; set; }
		public IEnumerable<FileInfo> pFiles { get; set; }
		public IEnumerable<Object_Object_Join> ObjectRealtions { get; set; }
		public IEnumerable<Object_Object_Join> RelatedObjects { get; set; }
        public IEnumerable<Object_Folder_Join> Folders { get; set; }
        public IEnumerable<Object_Folder_Join> FolderTree { get; set; }

		#endregion
		#region Constructor

		public Object()
		{
			pMetadatas = new List<Metadata>();
			pFiles     = new List<FileInfo>();
			ObjectRealtions = new List<Object_Object_Join>();
			RelatedObjects = new List<Object_Object_Join>();
            Folders = new List<Object_Folder_Join>();
            FolderTree = new List<Object_Folder_Join>();
		}

		#endregion
	}
}