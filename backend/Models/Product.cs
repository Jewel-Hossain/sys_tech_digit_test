//In the name of Allah

using System.ComponentModel.DataAnnotations.Schema;

namespace SysTech.Models;

public class Product
{
    public Guid? Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public double Rate { get; set; }
}//class

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).WithMessage("Rate must be greater than or equal to 0.");
    }//constructor
}//class

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product=> product.Id);
        builder.Property(product => product.Id).ValueGeneratedOnAdd();
        builder.HasIndex(product => product.Code).IsUnique();
    }//constructor
}//class
