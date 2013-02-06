namespace Chaos.Mcm.Data.Connection
{
    using System.Collections.Generic;

    public interface IKeyValueMapper
    {
        void Map(KeyValuePair<string, object>[] row);
    }
}