using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAW.Models
{
    public class Game
    {
        public int GameID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public int? GenreId { get; set; }

        [ValidateNever]
        public Genre Genre { get; set; }

        [ValidateNever]
        public string ImageFileName { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        [ValidateNever]
        public string ImagePath => "/images/" + (ImageFileName?.TrimStart('/') ?? "default.jpg");

        [ValidateNever]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [ValidateNever]
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        [ValidateNever]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [ValidateNever]
        public ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();
    }
}
