namespace Chaos.Mcm.Data.Connection
{
    using System.Collections.Generic;

    internal interface IMapping
    {
        object Map(KeyValuePair<string, object>[] row);
    }
}