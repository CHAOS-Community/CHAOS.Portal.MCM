namespace Chaos.Mcm.Extension.v5.Download
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Configuration;
    using Data;
    using Portal.Core;
    using Portal.Core.Data.Model;
    using Portal.Core.Extension.v5;
    using FileInfo = Data.Dto.FileInfo;

    public class Download : AExtension
    {
        private IMcmRepository McmRepository { get; set; }
        private McmModuleConfiguration McmModuleConfiguration { get; set; }
        private readonly IDictionary<string, IDownloadStrategy> _downloadStrategies = new Dictionary<string, IDownloadStrategy>();

        public Download(IPortalApplication portalApplication, IMcmRepository mcmRepository, McmModuleConfiguration mcmModuleConfiguration) : base(portalApplication, "MCM")
        {
            McmRepository = mcmRepository;
            McmModuleConfiguration = mcmModuleConfiguration;

            _downloadStrategies.Add("S3", new S3DownloadStrategy(mcmModuleConfiguration));
        }

        private Download(IPortalApplication portalApplication, IMcmRepository mcmRepository, McmModuleConfiguration mcmModuleConfiguration, IDictionary<string, IDownloadStrategy> downloadStrategies) : this(portalApplication, mcmRepository, mcmModuleConfiguration)
        {
            _downloadStrategies = downloadStrategies;
        }

        public Attachment Get(uint fileId)
        {
            var files = GetFiles(fileId);

            return GetStream(files);
        }

        private Attachment GetStream(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                if (!_downloadStrategies.ContainsKey(file.Token)) continue;
                
                var downloadStrategy = _downloadStrategies[file.Token];

                if (!downloadStrategy.AllowsDownload()) continue;

                return downloadStrategy.GetStream(file);
            }

            throw new ConfigurationErrorsException("No Access Provider associated with this file allows download through proxy");
        }

        private IEnumerable<FileInfo> GetFiles(uint fileId)
        {
            var file = McmRepository.FileGet(fileId);
            var obj = McmRepository.ObjectGet(file.ObjectGuid, false, true);

            var result = obj.Files.Where(item => item.Identifier == fileId);            

            return result;
        }

        public static Download CreateWithDownloadStrategy(IPortalApplication portalApplication, IMcmRepository mcmRepository, McmModuleConfiguration mcmModuleConfiguration, IDictionary<string, IDownloadStrategy> downloadStrategies)
        {
            return new Download(portalApplication, mcmRepository, mcmModuleConfiguration, downloadStrategies);
        }
    }
}
