using System;
using System.Collections.Generic;
using System.Linq;
using CHAOS.Data.MySql;
using Chaos.Mcm.Data.Dto;
using Chaos.Portal.Core.Exceptions;
using MySql.Data.MySqlClient;

namespace Chaos.Mcm.Data.MySql
{
  public class FileRepository : IFileRepository
  {
    public Gateway Gateway { get; set; }

    public FileRepository(Gateway gateway)
    {
      Gateway = gateway;
    }

    public uint Delete(uint id)
    {
      var result = Gateway.ExecuteNonQuery("File_Delete", new MySqlParameter("ID", id));

      return (uint)result;
    }

    public uint Create(Guid objectId, uint? parentId, uint destinationId, string filename, string originalFilename,
                       string folderPath, uint formatId)
    {
      var result = Gateway.ExecuteNonQuery("File_Create", new[]
        {
          new MySqlParameter("ObjectGuid", objectId.ToByteArray()),
          new MySqlParameter("ParentFileID", parentId),
          new MySqlParameter("FormatID", formatId),
          new MySqlParameter("DestinationID", destinationId),
          new MySqlParameter("Filename", filename),
          new MySqlParameter("OriginalFilename", originalFilename),
          new MySqlParameter("FolderPath", folderPath)
        });

      return (uint)result;
    }

    public uint Set(File file)
    {
      var result = Gateway.ExecuteNonQuery("File_Set", new[]
        {
          new MySqlParameter("Id", file.Id),
          new MySqlParameter("ObjectId", file.ObjectGuid.ToByteArray()),
          new MySqlParameter("ParentFileId", file.ParentID),
          new MySqlParameter("FormatId", file.FormatID),
          new MySqlParameter("DestinationId", file.DestinationID),
          new MySqlParameter("Filename", file.Filename),
          new MySqlParameter("OriginalFilename", file.OriginalFilename),
          new MySqlParameter("FolderPath", file.FolderPath)
        });

      return (uint)result;
    }

    public File Get(uint id)
    {
      var result = Gateway.ExecuteQuery<File>("File_Get", 
        new MySqlParameter("ID", id), new MySqlParameter("ParentId", null)).FirstOrDefault();

      if (result == null)
        throw new ChaosDatabaseException("No File by that Id");

      return result;
    }

    public IEnumerable<File> Get(uint? id = null, uint? parentId = null)
    {
      var result = Gateway.ExecuteQuery<File>("File_Get", new MySqlParameter("ID", id),
                                              new MySqlParameter("ParentId", parentId));

      return result;
    }
  }
}