using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDataSetApi.Models;

namespace HDataSetApi.Controllers{

    //response by default with a json data
    [Produce("application/json")]
    [Route("api/[controller]")]
    //This atrribute indicates that the controller correspond
    //to a WebApi
    [ApiController]
    public class HDataSetController : ControllerBase{

        private readonly HDataSetContext _context;

        //using Dependency Injection into the controller
        //The controller are calling the VegetableContext externally
        public HDataSetController(HDataSetContext context){

            _context = context;

            if (_context.Vegetables.Count() == 0){
                //Create a new Vegetable if collection is empty,
                //which means you can't delete all Vetegables
                _context.Vegetables.Add(new Vegetable {VegetableName = "Tomatoes"});
                _context.SaveChanges();
            }
        }

        //Get: api/hdataset
        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<Vegetable>>> GetVegetables(){
            return await _context.Vegetables.ToListAsync();
        }

        /// <summary>
        /// Get a specfic Vegetable by it Id
        /// </summary>
        /// <param name = "id"></param>
        /// <remarks>
        /// Sample request:
        ///     GET /hdataset
        ///     {
        ///         "id": 1,
        ///         "name": "Item1",   
        ///     }
        /// </remarks>
        //Get: api/hdataset/5
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<Vegetable>> GetVegetable(int id){
            var vegetable = await _context.Vegetables.FindAsync(id);

            if(vegetable == null){
                return NotFound();
            }

            return vegetable;
        }

        /// <summary>Created a Vegetable item</summary>
        /// <remarks>
        /// Sample request:
        ///     POST /hdataset
        ///     {
        ///         "id": 1,
        ///         "name": "Item1",
        ///         "isCompleted": true,
        ///     }
        /// </remarks>
        /// <param name = "item"></param>
        /// <response code = "201"> Returns the newly created item</response>
        /// <response code = "400"> If the item is null</response>
        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<Vegetable>> PostVegetable(Vegetable vegie){
            _context.Vegetables.Add(vegie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVegetable),new {id = vegie.Id}, vegie);
        }

        //PUT: api/hdataset/1
        [HttpPut("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PutVegetable(int id, Vegetable vegie){
            if(id != vegie.Id){
                return BadRequest();
            }

            _context.Entry(vegie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //DELETE: api/hdataset/5
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteVegetable(long id){

            var vegetable = await _context.Vegetables.FindAsync(id);

            if(vegetable == null){
                return NotFound();
            }

            _context.Vegetables.Remove(vegetable);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}