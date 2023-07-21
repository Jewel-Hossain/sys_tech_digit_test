//In the name of Allah


namespace SysTech.Models;

public class Customer
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}//class

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Phone).Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");
        RuleFor(x => x.Address).Length(0, 100).WithMessage("Address must be less than 100 characters.");
    }//constructor
}//class

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(customer => customer.Id).ValueGeneratedOnAdd();
    }//constructor
}//class

