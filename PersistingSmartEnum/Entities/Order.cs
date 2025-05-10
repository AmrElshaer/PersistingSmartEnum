using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersistingSmartEnum.Enums;

namespace PersistingSmartEnum.Entities;

public class Order
{
    public Guid Id { get; private set;  }
    public string CustomerName { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public OrderStatusEnum Status { get; private set; } = default!;

    public static Order Create(Guid id,string customerName,string address)
    {
        return new Order()
        {
            Id = id,
            CustomerName = customerName,
            Address = address,
            Status = OrderStatusEnum.Pending
        };
    }

    public void MarkAsShipped()
    {
        Status = OrderStatusEnum.Shipped;
    }

    public void MarkAsDelivered()
    {
        Status = OrderStatusEnum.Delivered;
    }

    public void MarkAsCancelled()
    {
        Status = OrderStatusEnum.Cancelled;
    }
}
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.CustomerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.Address)
            .IsRequired()
            .HasMaxLength(200);

        // Enum Mapping - assuming you're using SmartEnum with a value property
        builder.Property(o => o.Status)
            .HasConversion(
                status => status.Value, // Convert enum to int for storage
                value => OrderStatusEnum.FromValue(value)) // Convert int back to SmartEnum
            .IsRequired();
    }
}