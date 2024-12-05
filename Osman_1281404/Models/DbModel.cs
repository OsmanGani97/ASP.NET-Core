using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Osman_1281404.Models
{
    public enum RestaurantType { Fastfood=1, Buffet, Diner }
    public class Restaurant
    {
        public int RestaurantId {  get; set; }
        [Required, StringLength(60)]
        
        public string RestaurantName { get; set; } = default!;
        [Required, EnumDataType(typeof(RestaurantType))]
        public RestaurantType RestaurantType { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? OpeningDate { get; set; }
       

        [Required, StringLength(60)]
        public string Picture { get; set; } = default!;
        public bool IsAvailable { get; set; }
        public virtual ICollection<Food> Foods { get; set; } = new List<Food>();

    }
    public class Food
    {
        public int FoodId { get; set; }

       
        [Required, StringLength(60)]
        public string FoodName { get; set; } = default!;
        [Required, Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Required, ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; } = default!;
    }
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required, StringLength(60)]
        public string FoodName { get; set; } = default!;


    }
    public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
