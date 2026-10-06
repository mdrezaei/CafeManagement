using System.ComponentModel.DataAnnotations;

namespace MenuAndOrdering.API.Domain.Entities
{
    public class Table
    {
        [Key]
        public int Id { get; private set; }
        [Required]
        [MaxLength(100)]
        public string TableNumber { get; private set; }

        public bool IsDeleted { get; private set; }


        public Table(string tableNumber)
        {
            if(string.IsNullOrWhiteSpace(tableNumber))
            {
                throw new ArgumentException("شماره میز نمیتواند خالی باشد.");
            }

            TableNumber = tableNumber;

            IsDeleted = false;

        }

        public void UpdateTableNumber(string tableNumber)
        {
            if (string.IsNullOrWhiteSpace(tableNumber))
            {
                throw new ArgumentException("اسم و شماره میز نمیتواند خالی باشد.");
            }

            TableNumber = tableNumber;

        }
        public void SoftDelete()
        {
            IsDeleted = true;
        }


    }
}
