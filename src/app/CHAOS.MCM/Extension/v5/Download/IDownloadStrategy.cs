namespace Chaos.Mcm.Extension.v5.Download
{
    using Data.Dto;
    using Portal.Core.Data.Model;

    public interface IDownloadStrategy
    {
        Attachment GetStream(FileInfo file);
        bool AllowsDownload();
    }
}