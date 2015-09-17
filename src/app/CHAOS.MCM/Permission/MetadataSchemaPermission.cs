using System;

namespace Chaos.Mcm.Permission
{
    [Flags]
    public enum MetadataSchemaPermission : uint
    {
        None     = 0,
        Read     = 1 << 0,
        Write    = 1 << 1,
        Delete   = 1 << 2,
        WriteXml = 1 << 3,
        Max      = uint.MaxValue
    }
}