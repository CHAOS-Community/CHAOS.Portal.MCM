namespace Chaos.Mcm.Data.Connection
{
    using System.Data;

    public interface IReaderMapping
    {
        object Map(IDataReader reader);
    }
}