using Newtonsoft.Json;

namespace Chaos.Mcm.Data.Dto.v5
{
    using CHAOS.Serialization;
    using CHAOS.Serialization.XML;
    using Portal.Core.Data.Model;

    [Serialize("Result")]
    public class FileInfo : IResult
    {
        [Serialize("ID"), JsonProperty("ID")]
        public uint Id { get; set; }

        [Serialize]
        public uint? ParentID { get; set; }

        [Serialize]
        public string Filename { get; set; }

        [Serialize]
        public string OriginalFilename { get; set; }

        [Serialize]
        public string Token { get; set; }

        [Serialize("URL")]
        public string Url { get; set; }

        [Serialize]
        public uint FormatID { get; set; }

        [Serialize]
        public string Format { get; set; }

        [Serialize]
        public string FormatCategory { get; set; }

        [Serialize]
        public string FormatType { get; set; }

        [SerializeXML(true)]
        [Serialize("FullName")]
        public string Fullname { get; private set; }

        private FileInfo()
        {
            Fullname = "CHAOS.MCM.Data.DTO.FileInfo";
        }

        public static FileInfo Create(Dto.FileInfo item)
        {
            return new FileInfo
                {
                    Id = item.Identifier,
                    ParentID = item.ParentID,
                    Filename = item.Filename,
                    OriginalFilename = item.OriginalFilename,
                    Token = item.Token,
                    Url = item.URL,
                    FormatID = item.FormatID,
                    Format = item.Format,
                    FormatCategory = item.FormatCategory,
                    FormatType = item.FormatType
                };
        }
    }
}