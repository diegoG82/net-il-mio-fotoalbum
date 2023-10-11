using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }


        //Many to many relation with Photo
        public List<Photo> Photos { get; set; }

        //Empty construct
        public Category()
        {

        }
    }


}
