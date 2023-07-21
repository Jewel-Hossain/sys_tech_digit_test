//In the name of Allah

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CustomerController(ILogger<CustomerController> logger, ApplicationDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }//constructor

    [HttpGet("List")]
    public async Task<IActionResult> List() => Ok(await _context.Customers.ToListAsync());


    [HttpGet("Find")]
    public async Task<IActionResult> Find(Guid id) => Ok(await _context.Customers.FindAsync(id));


    [HttpPost("Post")]
    public async Task<IActionResult> Post(Customer customer)
    {
        var validator = new CustomerValidator();
        var validation_result = validator.Validate(customer);
        if (!validation_result.IsValid) return BadRequest(validation_result);
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Ok(customer);
    }//func


    [HttpPut("Put")]
    public async Task<IActionResult> Put(Customer customer)
    {

        _context.Entry(customer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();

        }//try
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Customers.Any(element => element.Id == customer.Id)) return NotFound();
            else throw;
        }//catch

        return NoContent();
    }//func

    [HttpPatch("Patch")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<Customer> patch_doc)
    {
        if (patch_doc == null) return BadRequest();

        var customer = await _context.Customers.FindAsync(id);

        if (customer == null) return NotFound();

         patch_doc.ApplyTo(customer);

        // patch_doc.ApplyTo(customer, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

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
            if (!_context.Customers.Any(c => c.Id == id))
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
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }//func
}//class
