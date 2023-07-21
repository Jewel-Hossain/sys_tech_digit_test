//In the name of Allah


using System.Text.Json.Serialization;

namespace SysTech.Models;

public class Inventory
{
    public Guid? Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public required Guid BillNo { get; set; }
    public double TotalDiscount { get; set; }
    public double TotalBillAmount { get; set; }
    public double DueAmount { get; set; }
    public double PaidAmount { get; set; }

    public List<InventoryProductMap> Products { get; set; } = new();

    [JsonIgnore]
    public Customer? Customer { get; set; }
}//class


public class InventoryValidator : AbstractValidator<Inventory>
{
    public InventoryValidator()
    {
        RuleFor(x => x.BillNo).NotEmpty().WithMessage("BillNo is required.");
        RuleFor(x => x.TotalBillAmount)
            .GreaterThanOrEqualTo(x => x.TotalDiscount + x.PaidAmount)
            .WithMessage("TotalBillAmount must be greater than or equal to the sum of TotalDiscount and PaidAmount.");
    }
}


public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(inventory => inventory.Id);
        builder.Property(inventory => inventory.Id).ValueGeneratedOnAdd();
        builder.HasOne(inventory => inventory.Customer)
                    .WithMany()
                    .HasForeignKey(inventory => inventory.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
    }//func
}//class

