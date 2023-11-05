using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentApi.Data;
using StudentEnrollmentApi.Models;

namespace StudentEnrollmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {

        private readonly StudentsDbContext _context;

        public MenuController(StudentsDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult GetMenu()
        {
            var content = _context.Menus.Include(b => b.MealType).ToList();

            if (content == null)
            {
                return BadRequest();
            }
            return Ok(content);
        }

        [HttpGet("{id}")]
        public IActionResult GetMenuById(int id)
        {
            var content = _context.Menus.FirstOrDefault(b => b.Id == id);
            if (content == null)
            {
                return BadRequest();
            }
            return Ok(content);
        }

        [HttpPost]
        public IActionResult CreateMenu([FromBody] Menu menu)
        {
            _context.Menus.Add(menu);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMenuById), new { id = menu.Id }, menu);
        }

        [HttpPost]
        [Route("FileUpload")]
        public async Task<IActionResult> CreateMenuWithFile([FromForm] MenuCreateDTO menuCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var studentImageFile = menuCreateDTO.MenuIdImageFile;

                if(studentImageFile != null && studentImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + "-" + studentImageFile.FileName;

                    var apiFilePath = Path.Combine("api", "server", "uploads", fileName);

                    using (var stream = new FileStream(apiFilePath, FileMode.Create))
                    {
                        await studentImageFile.CopyToAsync(stream);
                    }

                    Menu menu = new()
                    {
                        Starch = menuCreateDTO.Starch,
                        Meat = menuCreateDTO.Meat,
                        Beverage = menuCreateDTO.Beverage,
                        Vegetable = menuCreateDTO.Vegetable,
                        MealTypeId = menuCreateDTO.MealTypeId,
                        MenuImageFilePath = apiFilePath != String.Empty ? apiFilePath : ""
                    };

                    _context.Menus.Add(menu);
                    _context.SaveChanges();

                    return CreatedAtAction(nameof(GetMenuById), new { id = menu.Id }, menu);
                }
            }
            
            return BadRequest(ModelState);
        }

        [HttpGet("files/{filename}")]

        public IActionResult GetFile(string filename)
        {
            string filePath = Path.Combine("api", "server", "uploads", filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            string contentType = GetContentType(filePath);

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return new FileStreamResult(fileStream, contentType);
        }

        private string GetContentType(string filename)
        {
            string ext = Path.GetExtension(filename).ToLowerInvariant();

            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

        [HttpPut("{id}")]

        public IActionResult UpdateMenu(int id, [FromBody] Menu menu)
        {
            var content = _context.Menus.FirstOrDefault(x => x.Id == id);

            if (content == null)
            {
                return NotFound();
            }

            _context.Menus.Update(menu);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMenuById), new { id = menu.Id }, menu);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenu(int id)
        {
            if (_context.Menus == null)
            {
                return BadRequest("Entity set 'Menus' is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }

            await _context.SaveChangesAsync();
            return Ok(menu);
        }


        [HttpGet]
        [Route("mealType")]
        public IActionResult GetMealType()
        {
            var content = _context.MealTyes.ToList();

            if (content == null)
            {
                return BadRequest();
            }

            return Ok(content);
        }
    }



}
