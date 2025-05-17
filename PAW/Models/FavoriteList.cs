using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class FavoriteList
    {
        [Key]
        public int FavoriteListID { get; set; }

        public ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();
    }
}
