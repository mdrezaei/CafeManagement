using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CafeManagement.API.Domain.Entities
{
    public class MCustomer
    {
        [Key]
        public Guid Id { get; private set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }
        [Required]
        [MaxLength(25)]
        public string PhoneNumber { get; private set; }
        [MaxLength(256)]
        public string? Password { get; private set; }
        public DateTime SubmitDate { get; private set; }
        public bool IsDeleted { get; private set; }

        public MCustomer(string name, string phoneNumber, Guid id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("شماره تلفن همراه نمیتواند خالی باشد.");
            }

            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            SubmitDate = DateTime.UtcNow;
            IsDeleted = false;

        }

        public MCustomer(string name, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("شماره تلفن همراه نمیتواند خالی باشد.");
            }

            Id = Guid.NewGuid();
            Name = name;
            PhoneNumber = phoneNumber;
            SubmitDate = DateTime.UtcNow.Date;
            IsDeleted = false;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            
            Name = name;
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("شماره تلفن همراه نمیتواند خالی باشد.");
            }

            PhoneNumber = phoneNumber;
        }

        public void SetPassword(string password)
        {
            //امنیت پایین باید کلمه رمز ایمن بشه. هش کردن و اینجور چیزا
            //دو اینکه موقع پیاده سازی متد های رمز و اپدیت رمز استحکام رمز چک بشه و
            //با اتربیوت هایی مشخص بشه حداقل طولش چقدر باشه و چه کارکتر هایی داشته باشه
            Password = password;
        }

        public void UpdatePassword(string password)
        {
            //بجز موارد متد 
            //SetPassword
            //همچنین چک بشه مشابه رمز قبلی نباشه
            Password = password;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }

    }
}
