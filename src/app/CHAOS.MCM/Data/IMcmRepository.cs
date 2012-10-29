namespace CHAOS.MCM.Data
{
    public interface IMcmRepository
    {
        IMcmRepository WithConfiguration(string connectionString);
    }
}
