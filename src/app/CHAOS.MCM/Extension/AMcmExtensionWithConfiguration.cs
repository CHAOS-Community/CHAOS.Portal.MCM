using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CHAOS.Serialization.Standard.String;
using CHAOS.Serialization.Standard.XML;
using CHAOS.Serialization.XML;
using Chaos.Mcm.Data;
using Chaos.Mcm.Permission;
using Chaos.Portal.Core;


namespace Chaos.Mcm.Extension
{
	public abstract class AMcmExtensionWithConfiguration<T> : AMcmExtension
	{
		protected AMcmExtensionWithConfiguration(IPortalApplication portalApplication) : base(portalApplication)
		{
			
		}

		protected AMcmExtensionWithConfiguration(IPortalApplication portalApplication, IMcmRepository mcmRepository, IPermissionManager permissionManager) : base(portalApplication, mcmRepository, permissionManager)
		{
		}

		#region Configuration

		public T Configuration { get; private set; }

		private IXMLSerializer _serializer;
		protected IXMLSerializer Serializer
		{
			get
			{
				if (_serializer == null)
				{
					_serializer = new XMLSerializer(new StringSerializer());
					_serializer.Map(typeof(IList<>), typeof(List<>));
				}

				return _serializer;
			}
		}

		public AMcmExtensionWithConfiguration<T> WithConfiguration(string configuration)
		{
			Configuration = Serializer.Deserialize<T>(XDocument.Parse(configuration));

			return this;
		}

		#endregion
		#region GetFolderFromPath

		public Data.Dto.Standard.Folder GetFolderFromPath(bool failWhenMissing, string path)
		{
			return GetFolderFromPath(failWhenMissing, path.Split('/'));
		}

		public Data.Dto.Standard.Folder GetFolderFromPath(bool failWhenMissing, params string[] path)
		{
			var folders = McmRepository.FolderGet();

			if (folders == null)
				throw new System.Exception("No folders found");

			var folder = GetFolderFromPath(null, path.ToList(), folders);

			if (failWhenMissing && folder == null)
				throw new System.Exception("Could not find folder: " + path.Aggregate((a, e) => a + "/" + e));

			return folder;
		}

		private Data.Dto.Standard.Folder GetFolderFromPath(uint? parentId, IList<string> path, IList<Data.Dto.Standard.Folder> folders)
		{
			foreach (var folder in folders)
			{
				if (folder.ParentID == parentId && folder.Name == path[0])
				{
					if (path.Count == 1)
						return folder;

					path.RemoveAt(0);
					folders.Remove(folder);

					return GetFolderFromPath(folder.ID, path, folders);
				}
			}

			return null;
		}

		#endregion
	}
}