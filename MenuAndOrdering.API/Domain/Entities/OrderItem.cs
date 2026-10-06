using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAndOrdering.API.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; private set; }

        [Required]
        public int Quantity { get; private set; }

        [Required]
        public Guid MenuItemId { get; private set; }

        [Required]
        [MaxLength(100)]
        public string OrderedItemName { get; private set; }

        [Required]
        [Column(TypeName ="decimal(18,0)")]
        public decimal OrderedUnitPrice { get; private set; }

        private OrderItem()
        {
            //for ef core
        }
        public OrderItem(Guid menuItemId, string itemName, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("حداقل از ایتم مورد نظر باید یکی انتخاب شده باشد.");
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }

            if (unitPrice <= 0)
            {
                throw new ArgumentException("قیمت باید بیشتر از صفر باشد");
            }

            Id = Guid.NewGuid();
            Quantity = quantity;
            MenuItemId = menuItemId;
            OrderedItemName = itemName;
            OrderedUnitPrice = unitPrice;
        }

        public void DecreaseQuantity()
        {
            if(Quantity > 0)
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
