using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
