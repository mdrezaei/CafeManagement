using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagement.API.Domain.Entities
{
    public class MOrderItem
    {
        [Key]
        public Guid Id { get; private set; }
        public int Quantity { get; private set; }
        public Guid MenuItemId { get; private set; }
        [Required(ErrorMessage = "اسم ایتم الزامیست")]
        [MaxLength(100)]
        public string OrderedItemName { get; private set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal OrderedUnitPrice { get; private set; }

        public MOrderItem(int quantity, Guid menuItemId, string orderedItemName, decimal orderedUnitPrice)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("حداقل از ایتم مورد نظر باید یکی انتخاب شده باشد.");
            }
            if (string.IsNullOrWhiteSpace(orderedItemName))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if (orderedUnitPrice <= 0)
            {
                throw new ArgumentException("قیمت باید بیشتر از صفر باشد");
            }

            Id = Guid.NewGuid();
            Quantity = quantity;
            MenuItemId = menuItemId;
            OrderedItemName = orderedItemName;
            OrderedUnitPrice = orderedUnitPrice;
        }

        public void DecreaseQuantity()
        {
            if (Quantity > 0)
            {
                Quantity -= 1;
            }
        }

        public void DecreaseQuantity(int decrease)
        {
            if (decrease <= 0)
            {
                throw new ArgumentException("باید بیشتر از صفر باشد عدد ورودی.");
            }

            if (decrease > Quantity)
            {
                Quantity = 0;
            }
            else
            {
                Quantity -= decrease;
            }
        }

        public void AddQuantity()
        {
            Quantity += 1;
        }

        public void AddQuantity(int add)
        {
            if (add <= 0)
            {
                throw new ArgumentException("باید بیشتر از صفر باشد عدد ورودی.");
            }

            Quantity += add;
        }

    }
}
