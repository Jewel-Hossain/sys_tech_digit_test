//In the name of Allah

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductController(ILogger<ProductController> logger, ApplicationDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }//constructor

    [HttpGet("List")]
    public async Task<IActionResult> List() => Ok(await _context.Products.ToListAsync());


    [HttpGet("Find")]
    public async Task<IActionResult> Find(Guid id) => Ok(await _context.Products.FindAsync(id));


    [HttpPost("Post")]
    public async Task<IActionResult> Post(Product Product)
    {
        var validator = new ProductValidator();
        var validation_result = validator.Validate(Product);
        if (!validation_result.IsValid) return BadRequest(validation_result);
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return Ok(Product);
    }//func


    [HttpPut("Put")]
    public async Task<IActionResult> Put(Product Product)
    {

        _context.Entry(Product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

        }//try
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(element => element.Id == Product.Id)) return NotFound();
            else throw;
        }//catch

        return NoContent();
    }//func

    [HttpPatch("Patch")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<Product> patch_doc)
    {
        if (patch_doc == null) return BadRequest();

        var Product = await _context.Products.FindAsync(id);

        if (Product == null) return NotFound();

         patch_doc.ApplyTo(Product);

        // patch_doc.ApplyTo(Product, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(c => c.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Product = await _context.Products.FindAsync(id);
        if (Product == null) return NotFound();

        _context.Products.Remove(Product);
        await _context.SaveChangesAsync();

        return NoContent();
    }//func
}//class
