namespace CHAOS.MCM.Data.EF
{
    public class McmRepository : IMcmRepository
    {
        #region Fields

        private string _connectionString;

        #endregion
        #region Properties



        #endregion
        #region Construction

        public IMcmRepository WithConfiguration(string connectionString)
        {
            _connectionString = connectionString;

            return this;
        }

        private MCMEntities CreateMcmEntities()
        {
            return new MCMEntities(_connectionString);
        }

        #endregion
        #region Business Logic

        

        #endregion
    }
}
