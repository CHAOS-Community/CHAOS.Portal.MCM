using System;
using System.Collections.Generic;
using Chaos.Portal;
using Chaos.Portal.Core.Cache.Couchbase;
using Chaos.Portal.Core.Data;
using Chaos.Portal.Core.Indexing.View;
using Chaos.Portal.Core.Logging.Database;

namespace Chaos.Mcm.Indexer
{
  class Program
  {
    static void Main(string[] args)
    {
      var couchbaseConfig = new Couchbase.Configuration.CouchbaseClientConfiguration();
      couchbaseConfig.Bucket = "larm";
      couchbaseConfig.Urls.Add(new Uri("http://10.0.252.63:8091/pools"));
      var cache = new Cache(new Couchbase.CouchbaseClient(couchbaseConfig));
      var portalRepository = new PortalRepository().WithConfiguration("user id=larm-app;password=0n44Fx4f4m2jNtuLuA6ym88mr3h40D;server=mysql01.cpwvkgghf9fg.eu-west-1.rds.amazonaws.com;persist security info=True;database=larm-portal;Allow User Variables=True;CharSet=utf8;");
      var portal = new PortalApplication(cache, new ViewManager(new Dictionary<string, IView>(), cache), portalRepository, new DatabaseLoggerFactory(portalRepository));
      var mcm = new McmModule();
      mcm.Load(portal);

      const uint PageSize = 100;
      var indexedCount = 0;

      for (uint i = 0; ; i++)
      {
        var objects = mcm.McmRepository.ObjectGet(null, i, PageSize, true, true, true, true, true, null);

        portal.ViewManager.GetView("Search").Index(objects);
        portal.ViewManager.GetView("Object").Index(objects);

        Console.SetCursorPosition(0,Console.CursorTop);
        Console.Write("Objects indexed: {0}", ++indexedCount);
        if (objects.Count != PageSize) break;
      }
    }
  }
}
