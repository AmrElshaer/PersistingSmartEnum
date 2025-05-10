using Ardalis.SmartEnum;
using PersistingSmartEnum.Enums;

namespace PersistingSmartEnum.Entities;
public abstract class OrderStatusEnum(string name, int value) : SmartEnum<OrderStatusEnum>(name, value)
{
    public static readonly OrderStatusEnum Pending = new PendingType();
    public static readonly OrderStatusEnum Approved = new ApprovedType();
    public static readonly OrderStatusEnum Rejected = new RejectedType();
    public static readonly OrderStatusEnum Shipped = new ShippedType();
    public static readonly OrderStatusEnum Delivered = new DeliveredType();
    public static readonly OrderStatusEnum Cancelled = new CancelledType();
    
    public abstract IEnumerable<OrderStatusLocalization> GetLocalizations();

    private sealed class PendingType() : OrderStatusEnum("Pending", 1)
    {
        public override IReadOnlyList<OrderStatusLocalization> GetLocalizations() => [
            new() { Id = 1, OrderStatus = this, Language = LanguageEnum.Arabic, Name = "قيد الانتظار" },
            new() { Id = 2, OrderStatus = this, Language = LanguageEnum.English, Name = Name }
        ];
    }

    private sealed class ApprovedType() : OrderStatusEnum("Approved", 2)
    {
        public override IReadOnlyList<OrderStatusLocalization> GetLocalizations() => [
            new() { Id = 3, OrderStatus = this, Language = LanguageEnum.Arabic, Name = "تمت الموافقة" },
            new() { Id = 4, OrderStatus = this, Language = LanguageEnum.English, Name = Name }
        ];
    }

    private sealed class RejectedType() : OrderStatusEnum("Rejected", 3)
    {
        public override IReadOnlyList<OrderStatusLocalization> GetLocalizations() => [
            new() { Id = 5, OrderStatus = this, Language = LanguageEnum.Arabic, Name = "مرفوض" },
            new() { Id = 6, OrderStatus = this, Language = LanguageEnum.English, Name = Name }
        ];
    }

    private sealed class ShippedType() : OrderStatusEnum("Shipped", 4)
    {
        public override IReadOnlyList<OrderStatusLocalization> GetLocalizations() => [
            new() { Id = 7, OrderStatus = this, Language = LanguageEnum.Arabic, Name = "تم الشحن" },
            new() { Id = 8, OrderStatus = this, Language = LanguageEnum.English, Name = Name }
        ];
    }

    private sealed class DeliveredType() : OrderStatusEnum("Delivered", 5)
    {
        public override IReadOnlyList<OrderStatusLocalization> GetLocalizations() => [
            new() { Id = 9, OrderStatus = this, Language = LanguageEnum.Arabic, Name = "تم التوصيل" },
            new() { Id = 10, OrderStatus = this, Language = LanguageEnum.English, Name = Name }
        ];
    }

    private sealed class CancelledType() : OrderStatusEnum("Cancelled", 6)
    {
        public override IReadOnlyList<OrderStatusLocalization> GetLocalizations() => [
            new() { Id = 11, OrderStatus = this, Language = LanguageEnum.Arabic, Name = "ملغي" },
            new() { Id = 12, OrderStatus = this, Language = LanguageEnum.English, Name = Name }
        ];
    }
}
public class OrderStatusLocalization
{
    public int Id { get; init; }
    public required string Name { get; init; } 
    public required LanguageEnum Language { get; init; }
    public required OrderStatusEnum OrderStatus { get; init; } 
}

