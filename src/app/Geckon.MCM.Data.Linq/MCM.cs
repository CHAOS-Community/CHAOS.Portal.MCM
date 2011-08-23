using Geckon.Serialization.Xml;

namespace Geckon.MCM.Data.Linq
{
    partial class MCMDataContext
    {

    }

    [Document("Geckon.MCM.Data.Linq.DTO.ObjectType")]
    partial class ObjectType : XmlSerialize
    {
        #region Properties

        [Element]
        public int pID
        {
            get { return ID; }
        }

        [Element]
        public string pValue
        {
            get { return Value; }
        }

        #endregion
    }

    [Document("Geckon.MCM.Data.Linq.Language")]
    partial class Language : XmlSerialize
    {
        #region Properties

        [Element("ID")]
        public int pID
        {
            get { return ID; }
        }

        [Element("Name")]
        public string pName
        {
            get { return Name; }
        }

        [Element("LanguageCode")]
        public string pLanguageCode
        {
            get { return LanguageCode; }
        }

        [Element("CountryName")]
        public string pCountryName
        {
            get { return CountryName; }
        }

        #endregion
    }
}
