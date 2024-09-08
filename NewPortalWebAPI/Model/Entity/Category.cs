using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewPortalWebAPI.Model.Entity
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        //It has one to many relation with news
        [JsonIgnore]
        public ICollection<NewsInfo> NewsInfo{ get; } = new List<NewsInfo>();
    }
}
