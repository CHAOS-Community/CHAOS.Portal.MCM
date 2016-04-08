using Chaos.Mcm.Data.Dto;
using Chaos.Portal.Core.Data.Model;

namespace Chaos.Mcm.Extension
{
	public interface IDownloadStrategy
    {
        Attachment GetStream(FileInfo file);
        bool AllowsDownload();
    }
}