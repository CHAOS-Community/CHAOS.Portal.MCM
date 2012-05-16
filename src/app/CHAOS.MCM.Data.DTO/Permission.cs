using CHAOS.Serialization;

namespace CHAOS.MCM.Data.DTO
{
    public class Permission
    {
        #region Properties

        [Serialize]
        public string Name { get; set; }

        [Serialize]
        public uint Value { get; set; }

        #endregion
        #region Constructors

        public Permission()
        {
            
        }

        public Permission( string name, uint value )
        {
            Name  = name;
            Value = value;
        }

        #endregion
    }
}
