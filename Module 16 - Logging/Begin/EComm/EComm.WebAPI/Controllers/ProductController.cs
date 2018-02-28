using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComm.WebAPI.Controllers
{
    //[Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private DataContext ECommDataContext { get; }
        public ProductController(DataContext dataContext)
        {
            ECommDataContext = dataContext;
        }

        // GET: api/Product
        //[HttpGet]
        //public IEnumerable<Product> Get()
        //{
        //    return ECommDataContext.Products.ToList();
        //}
        // Lambda Style
        [HttpGet]
        public IEnumerable<Product> Get() => ECommDataContext.Products.ToList();

        // GET: api/Product/5
        [HttpGet("{id}/{format?}", Name = "Get")]
        [FormatFilter]
        [Produces("application/json", "application/xml")]
        public IActionResult Get(int id)
        {
            var product = ECommDataContext.Products
                //.Include(p => p.Supplier)
                .SingleOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            // Helper Method
            //return Ok(product);

            // ObjectResult base class for Helper Methods
            return new ObjectResult(product);
        }
        
        // POST: api/Product
        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ProductExists(product.Id)) return StatusCode(StatusCodes.Status409Conflict);

            try
            {
                ECommDataContext.Products.Add(product);
                ECommDataContext.SaveChanges();
            }
            catch (DbUpdateException) { return StatusCode(StatusCodes.Status409Conflict); }
            catch (Exception) { return StatusCode(StatusCodes.Status500InternalServerError); }

            return CreatedAtAction("Get", new { id = product.Id }, product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if (product == null || id != product.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!ProductExists(id)) return NotFound();

            var existing = ECommDataContext.Products.SingleOrDefault(p => p.Id == id);

            try
            {
                existing.ProductName = product.ProductName;
                existing.UnitPrice = product.UnitPrice;
                existing.Package = product.Package;
                existing.IsDiscontinued = product.IsDiscontinued;
                existing.SupplierId = product.SupplierId;
                ECommDataContext.SaveChanges();
            }
            catch (DbUpdateException) { return StatusCode(StatusCodes.Status409Conflict); }
            catch (Exception) { return StatusCode(StatusCodes.Status500InternalServerError); }

            return Ok(existing);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ProductExists(id)) return NotFound();
            var existing = ECommDataContext.Products.SingleOrDefault(p => p.Id == id);

            try
            {
                ECommDataContext.Remove(existing);
                ECommDataContext.SaveChanges();
            }
            catch (DbUpdateException) { return StatusCode(StatusCodes.Status409Conflict); }
            catch (Exception) { return StatusCode(StatusCodes.Status500InternalServerError); }

            return Ok(existing);
        }

        private bool ProductExists(int id) => ECommDataContext.Products.Count(p => p.Id == id) > 0;
    }
}
