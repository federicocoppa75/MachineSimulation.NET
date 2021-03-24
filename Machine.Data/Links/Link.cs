using Machine.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Machine.Data.Links
{
    public abstract class Link
    {
        [Key]
        public int LinkID { get; set; }
        public int Id { get; set; }
        public LinkDirection Direction { get; set; }
        public LinkType Type { get; set; }
    }
}
