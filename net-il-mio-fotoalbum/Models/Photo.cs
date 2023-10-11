using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Image { get; set; }

        public bool? Visible { get; set; }


        //Many to many relation with categories
        public List<Category>? categories { get; set; }
     

        //Empty construct
        public Photo() { }


        Photo (string title, string description, string image, bool visible)

        {
            this.Title = title;
            this.Description = description;
            this.Image = image;
            this.Visible = visible;
        }
    }
}