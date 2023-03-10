using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoriesController:ControllerBase
    {
        private readonly DataContext _conext;

        public CategoriesController(DataContext conext)
        {
            _conext = conext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _conext.Categories.ToListAsync());
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var category = await _conext.Categories.FindAsync(id);
            if (category==null)
            {
                return NotFound();
            }
            return Ok(category);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(Category category)
        {

            _conext.Add(category);

            try
            {
                await _conext.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una categoría con el mismo nombre");
                }
                else
                {
                    return BadRequest(dbUpdateException.Message);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<ActionResult> PutAsync(Category category)
        {
            _conext.Update(category);

            try
            {
                await _conext.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una categoría con el mismo nombre");
                }
                else
                {
                    return BadRequest(dbUpdateException.Message);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> DeleteAsyn(int id)
        {

            var category = await _conext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _conext.Categories.Remove(category);
            await _conext.SaveChangesAsync();
            return NoContent();
        }
    }
}
