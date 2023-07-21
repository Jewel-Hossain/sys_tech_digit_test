//In the name of Allah

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
    private readonly ILogger<InventoryController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InventoryController(ILogger<InventoryController> logger, ApplicationDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }//constructor

    [HttpGet("List")]
    public async Task<IActionResult> List() => Ok(await _context.Inventories.Include(element => element.Products).ToListAsync());


    [HttpGet("Find")]
    public async Task<IActionResult> Find(Guid id) => Ok(await _context.Inventories.Include(element => element.Products).FirstOrDefaultAsync(element => element.Id == id));


    [HttpPost("Post")]
    public async Task<IActionResult> Post(Inventory Inventory)
    {
        var validator = new InventoryValidator();
        var validation_result = validator.Validate(Inventory);
        if (!validation_result.IsValid) return BadRequest(validation_result);
        _context.Inventories.Add(Inventory);
        await _context.SaveChangesAsync();

        return Ok(Inventory);
    }//func


    [HttpPut("Put")]
    public async Task<IActionResult> Put(Inventory Inventory)
    {

        _context.Entry(Inventory).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

        }//try
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Inventories.Any(element => element.Id == Inventory.Id)) return NotFound();
            else throw;
        }//catch

        return NoContent();
    }//func

    [HttpPatch("Patch")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<Inventory> patch_doc)
    {
        if (patch_doc == null) return BadRequest();

        var Inventory = await _context.Inventories.FindAsync(id);

        if (Inventory == null) return NotFound();

         patch_doc.ApplyTo(Inventory);

        // patch_doc.ApplyTo(Inventory, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

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
            if (!_context.Inventories.Any(c => c.Id == id))
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
        var Inventory = await _context.Inventories.FindAsync(id);
        if (Inventory == null) return NotFound();

        _context.Inventories.Remove(Inventory);
        await _context.SaveChangesAsync();

        return NoContent();
    }//func
}//class
