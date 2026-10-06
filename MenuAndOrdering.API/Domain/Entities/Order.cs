using MenuAndOrdering.API.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAndOrdering.API.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; private set; }
        public List<OrderItem> OrderItems { get; private set; }
        [Required]
        public OrderStatus Status { get; private set; }
        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                return OrderItems.Sum(o => o.OrderedUnitPrice * o.Quantity);
            }
        }
        [MaxLength(500)]
        public string? CustomerNote { get; private set; }
        [Required]
        public Guid CustomerId { get; private set; }
        [Required]
        public int TableId { get; private set; }
        [Required]
        public DateTime CreatedDate { get; private set; }

        public bool IsDeleted { get; private set; }

        public Order(Guid customerId, int tableId, string? customerNote = null)
        {
            if (customerId == Guid.Empty)
            {
                throw new ArgumentException("شناسه مشتری نمی‌تواند خالی باشد.");
            }

            if (tableId <= 0)
            {
                throw new ArgumentException("شماره میز نمی‌تواند نامعتبر باشد.");
            }


            Id = Guid.NewGuid();
            CustomerId = customerId;
            TableId = tableId;
            CustomerNote = customerNote;
            Status = OrderStatus.Placed;

            OrderItems = new List<OrderItem>();

            CreatedDate = DateTime.UtcNow;

            IsDeleted = false;

        }


        public void AddItem(Guid menuItemId, string itemName, decimal unitPrice, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("تعداد باید بیشتر از صفر باشد.");
            }

            OrderItem? existingItem = OrderItems.FirstOrDefault(o => o.MenuItemId == menuItemId);

            if (existingItem != null)
            {
                existingItem.AddQuantity(quantity);
            }
            else
            {
                OrderItem newItem = new OrderItem(menuItemId, itemName, unitPrice, quantity);
                OrderItems.Add(newItem);
            }

        }

        public void RemoveItem(Guid menuItemId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("باید بیشتر از صفر باشد.");
            }

            OrderItem? existingItem = OrderItems.FirstOrDefault(o => o.MenuItemId == menuItemId);

            if (existingItem != null)
            {
                existingItem.DecreaseQuantity(quantity);
            }
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            if ((int)newStatus != (int)Status + 1)
            {
                throw new InvalidOperationException($"امکان تغییر وضعیت از {Status} به {newStatus} وجود ندارد.");
            }
            else
            {
                Status = newStatus;
            }

        }

        public void ClearEmptyItems()
        {
            OrderItems.RemoveAll(o => o.Quantity == 0);
        }

        public void ChangeTable(int tableId)
        {
            if (tableId <= 0)
            {
                throw new ArgumentException("شماره میز نامعتبر است.");
            }

            TableId = tableId;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }


    }
}
