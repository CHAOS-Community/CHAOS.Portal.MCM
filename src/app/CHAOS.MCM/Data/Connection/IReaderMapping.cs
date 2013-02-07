namespace Chaos.Mcm.Data.Connection
{
    using System.Collections.Generic;
    using System.Data;

    public interface IReaderMapping<out TResult>
    {
        IEnumerable<TResult> Map(IDataReader reader);
    }

    public interface IReaderMapping : IReaderMapping<object>
    {
    }
}