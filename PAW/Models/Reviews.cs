using System;
using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public int GameID { get; set; }
        public Game Game { get; set; }
        public string UserId { get; set; }
    }
}
