using System;
using System.Collections.Generic;
using Chaos.Mcm.Data.Dto;

namespace Chaos.Mcm.Data
{
  public interface IFileRepository
  {
    uint Delete(uint id);

    uint Create(Guid objectId, uint? parentId, uint destinationId, string filename, string originalFilename, string folderPath, uint formatId);

    uint Set(File file);

    IEnumerable<File> Get(uint? id = null, uint? parentId = null);

    File Get(uint id);
  }
}