using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewPortalWebAPI.Model.Entity
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        public String Category_Name { get; set; }
    }
}
