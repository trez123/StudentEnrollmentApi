using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentApi.Data;
using StudentEnrollmentApi.Models;

namespace StudentEnrollmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentsDbContext _context;

        public StudentController(StudentsDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var content = _context.Students.Include(b => b.Parish).Include(b => b.Size).Include(b => b.Course).ToList();

            if (content == null)
            {
                return BadRequest();
            }
            return Ok(content);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var content = _context.Students.FirstOrDefault(b => b.Id == id);
            if (content == null)
            {
                return BadRequest();
            }
            return Ok(content);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            var content = _context.Students.FirstOrDefault(x => x.Id == id);

            if (content == null)
            {
                return NotFound();
            }

            _context.Students.Update(student);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            if (_context.Students == null)
            {
                return BadRequest("Entity set 'Students' is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return Ok(student);
        }


        [HttpGet]
        [Route("course")]
        public IActionResult GetCourse()
        {
            var content = _context.Courses.ToList();

            if (content == null)
            {
                return BadRequest();
            }

            return Ok(content);
        }

        [HttpGet]
        [Route("parish")]
        public IActionResult GetParish()
        {
            var content = _context.Parishes.ToList();

            if (content == null)
            {
                return BadRequest();
            }

            return Ok(content);
        }

        [HttpGet]
        [Route("size")]
        public IActionResult GetSize()
        {
            var content = _context.Sizes.ToList();

            if (content == null)
            {
                return BadRequest();
            }

            return Ok(content);
        }
    }



}
