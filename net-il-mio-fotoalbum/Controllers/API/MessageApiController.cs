using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageApiController : ControllerBase
    {
        private PhotoContext _myDb;

        public MessageApiController(PhotoContext myDb)
        {
            _myDb = myDb;
        }

        [HttpPost]
        public IActionResult StoreMessage([FromBody] Message message)
        {
            if (message == null)
            {
                return BadRequest("Dati non validi");
            }

            _myDb.Messages.Add(message);
            _myDb.SaveChanges();

            return Ok(new { success = true });
        }

    }
}
