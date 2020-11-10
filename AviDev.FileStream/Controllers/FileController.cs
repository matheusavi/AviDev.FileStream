using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AviDev.FileStream
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly AviDevFileStreamContext _context;

        public FileController(AviDevFileStreamContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<File>> Get(int id)
        {
            var file = await _context.File.FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpPost]
        public async Task<ActionResult<File>> Post([FromForm]IFormFile file)
        {
            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            var addedEntity = _context.File.Add(new File(memoryStream.ToArray(), file.FileName));

            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = addedEntity.Entity.Id }, addedEntity.Entity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<File>> Delete(int id)
        {
            var file = await _context.File.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }

            _context.File.Remove(file);
            await _context.SaveChangesAsync();

            return file;
        }

        private bool FileExists(int id)
        {
            return _context.File.Any(e => e.Id == id);
        }
    }
}
