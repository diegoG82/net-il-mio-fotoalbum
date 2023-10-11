using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.API

{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotosApiController : ControllerBase

    {
        private PhotoContext _myDb;

        public PhotosApiController(PhotoContext myDb)
        {
            _myDb = myDb;
        }

        //LISTA DI TUTTE LE FOTO
        [HttpGet]
        public IActionResult GetPhotos()
        {

            List<Photo> photos = _myDb.Photos.Include(p => p.categories).ToList();

            if (photos.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(photos);
            }

        }


        //FILTRO CON TITOLO
        [HttpGet]
        public IActionResult GetPhotosByTitle(string? title)
        {
            if (title == null)
            {
                return GetPhotos();
            }

            List<Photo> photos = _myDb.Photos.Where(photo => photo.Title.Contains(title)).Include(photo => photo.categories).ToList();

            if (photos.Count == 0)
            {
                return GetPhotos();
            }
            else
            {
                return Ok(photos);
            }

        }
    }
}