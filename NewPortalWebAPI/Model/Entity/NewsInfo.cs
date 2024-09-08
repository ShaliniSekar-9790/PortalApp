using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewPortalWebAPI.Model.Entity
{
    public class NewsInfo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
