using CafeManagement.API.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagement.API.Domain.Entities
{
    public class MOrder
    {
        [Key]
        public Guid Id { get; private set; }
        public List<MOrderItem> OrderItems { get; private set; }
        public MOrderStatus Status { get; private set; }
        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                return OrderItems.Sum(o => o.Quantity * o.OrderedUnitPrice);
            }
        }
        public Guid CustomerId { get; private set; }
        [MaxLength(500)]
        public string? CustomerNote { get; private set; }
        public int TableId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public Guid SourceOrderId { get; private set; }
        public Guid? AssignedWaiterId { get; private set; }
        public Guid? AssignedBaristaId { get; private set; }
        public Guid? AssignedChefId { get; private set; }
        public Guid? AssignedCashierId { get; private set; }
        public bool IsDeleted { get; private set; }

        public MOrder(Guid sourceOrderId, int tableId, Guid customerId)
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
            OrderItems = new List<MOrderItem>();
            Status = MOrderStatus.Placed;
            CustomerId = customerId;
            TableId = tableId;
            CreatedDate = DateTime.UtcNow.Date;
            SourceOrderId = sourceOrderId;
            IsDeleted = false;
        }

        //we can use these methodes for assign and update Employies working on this order
        public void AssignWaiter(Guid id)
        {
            AssignedWaiterId = id;
        }

        public void AssignBarista(Guid id)
        {
            AssignedBaristaId = id;
        }

        public void AssignChef(Guid id)
        {
            AssignedChefId = id;
        }

        public void AssignCashier(Guid id)
        {
            AssignedCashierId = id;
        }

        public void AddItem(Guid menuItemId, string orderedItemName, decimal orderedUnitPrice, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("تعداد باید بیشتر از صفر باشد.");
            }

            MOrderItem? existingItem = OrderItems.FirstOrDefault(o => o.MenuItemId == menuItemId);

            if (existingItem != null)
            {
                existingItem.AddQuantity(quantity);
            }
            else
            {
                MOrderItem newItem = new MOrderItem(quantity, menuItemId, orderedItemName, orderedUnitPrice);
                OrderItems.Add(newItem);
            }

        }

        public void RemoveItem(Guid menuItemId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("تعداد باید بیشتر از صفر باشد.");
            }

            MOrderItem existingItem = OrderItems.FirstOrDefault(o => o.MenuItemId == menuItemId);

            if (existingItem != null)
            {
                existingItem.DecreaseQuantity(quantity);
            }

        }

        public void ClearEmptyItems()
        {
            OrderItems.RemoveAll(o => o.Quantity == 0);
        }

        public void UpdateStatus(MOrderStatus newStatus)
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

        public void ChangeTable(int tableId)
        {
            if (tableId <= 0)
            {
                throw new ArgumentException("شماره میز نامعتبر است.");
            }

            TableId = tableId;
        }

        public void SetCustomerNote(string Note)
        {
            CustomerNote = Note;
        }

        public void UpdateSourceOrder(Guid id)
        {
            SourceOrderId = id;
        }

        public void UpdateCustomer(Guid id)
        {
            CustomerId = id;
        }

        public void UpdateCreatedDate(DateTime createdDate)
        {
            CreatedDate = createdDate.Date;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }

    }
}
