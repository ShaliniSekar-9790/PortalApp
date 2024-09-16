using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewPortalWebAPI.Model.Entity
{
    public class NewsInfo
    {
        [Key]
        public int News_Id { get; set; }
        public string Title { get; set; }
        public string News_Description { get; set; }
        public int Category_Id { get; set; }
        public DateTime Create_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        [NotMapped]
        public virtual Category Category { get; set; }

    }
}
