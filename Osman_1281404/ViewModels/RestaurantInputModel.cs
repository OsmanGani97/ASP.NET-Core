using Osman_1281404.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Osman_1281404.ViewModels
{
    public class RestaurantInputModel
    {
        public int RestaurantId { get; set; }
        [Required, StringLength(40)]

        public string RestaurantName { get; set; } = default!;
        [Required, EnumDataType(typeof(RestaurantType))]
        public RestaurantType RestaurantType { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? OpeningDate { get; set; }
       

        [Required]
        public IFormFile Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public List<Food> Foods { get; set; } = new List<Food>();
    }
}
