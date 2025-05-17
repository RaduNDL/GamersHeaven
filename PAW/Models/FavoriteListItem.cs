using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class FavoriteItem
    {
        [Key]
        public int FavoriteItemID { get; set; }

        public int GameID { get; set; }
        public Game Game { get; set; }

        public string? GameTitle { get; set; }

        public int FavoriteListID { get; set; }
        public FavoriteList FavoriteList { get; set; }
    }
}
