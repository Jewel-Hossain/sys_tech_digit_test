//In the name of Allah


using System.Text.Json.Serialization;

namespace SysTech.Models;

public class InventoryProductMap
{
    public Guid? Id { get; set; }
    public Guid InventoryId { get; set; }
    public Guid ProductId { get; set; }
    public double Rate { get; set; }
    public double Qty { get; set; }
    public double Discount { get; set; }

    [JsonIgnore]
    public Inventory? Inventory { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }
}//class

public class InventoryProductMapValidator : AbstractValidator<InventoryProductMap>
{
    public InventoryProductMapValidator()
    {
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).WithMessage("Rate must be greater than or equal to 0.");
        RuleFor(x => x.Qty).GreaterThanOrEqualTo(0).WithMessage("Qty must be greater than or equal to 0.");
        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Discount must be greater than or equal to 0.");
    }//constructor
}//class

public class InventoryProductMapConfiguration : IEntityTypeConfiguration<InventoryProductMap>
{
    public void Configure(EntityTypeBuilder<InventoryProductMap> builder)
    {
        builder.HasKey(inventory_product_map => inventory_product_map.Id);
        builder.Property(inventory_product_map => inventory_product_map.Id).ValueGeneratedOnAdd();
        builder.HasOne(inventory_product_map => inventory_product_map.Inventory)
                    .WithMany()
                    .HasForeignKey(inventory_product_map => inventory_product_map.InventoryId)
                    .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(inventory_product_map => inventory_product_map.Product)
                    .WithMany()
                    .HasForeignKey(inventory_product_map => inventory_product_map.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
    }//constructor
}//class

