using CafeManagement.API.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace CafeManagement.API.Domain.Entities
{
    public class MEmployee
    {
        [Key]
        public Guid Id { get; private set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; private set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; private set; }
        [Required]
        [MaxLength(25)]
        public string PhoneNumber { get; private set; }
        public DateTime JoinedDate { get; private set; }
        public EmployeeSections Section { get; private set; }
        public EmployeeLevels Level { get; private set; }
        public EmployeeSections? SecondarySection { get; private set; }
        public EmployeeLevels? SecondaryLevel { get; private set; }
        public bool IsDeleted { get; private set; }
        [MaxLength(500)]
        public string? Description { get; private set; }

        public MEmployee(string name, string password, string phoneNumber, EmployeeSections section, EmployeeLevels level)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("رمز نمیتواند خالی باشد.");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("شماره تلفن نمیتواند خالی باشد.");
            }

            Id = Guid.NewGuid();
            Name = name;
            Password = password;
            PhoneNumber = phoneNumber;
            JoinedDate = DateTime.UtcNow.Date;
            Section = section;
            Level = level;
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

        public void UpdatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("رمز نمیتواند خالی باشد.");
            }

            Password = password;
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                throw new ArgumentException("شماره تلفن نمیتواند خالی باشد.");
            }

            PhoneNumber = phoneNumber;
        }

        public void UpdateJoinedDate(DateTime joinedDate)
        {
            JoinedDate = joinedDate.Date;
        }

        public void UpdateSection(EmployeeSections employeeSection)
        {
            Section = employeeSection;
        }

        public void SetSecondarySection(EmployeeSections employeeSection)
        {
            SecondarySection = employeeSection;
        }

        public void UpdateSecondarySection(EmployeeSections employeeSection)
        {
            SecondarySection = employeeSection;
        }

        public void DeleteSecondarySection()
        {
            SecondarySection = null;
        }

        public void UpdateLevel(EmployeeLevels employeeLevel)
        {
            Level = employeeLevel;
        }

        public void SetSecondaryLevel(EmployeeLevels employeeLevel)
        {
            SecondaryLevel = employeeLevel;
        }

        public void UpdateSecondaryLevel(EmployeeLevels employeeLevel)
        {
            SecondaryLevel = employeeLevel;
        }

        public void DeleteSecondaryLevel()
        {
            SecondaryLevel = null;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void DeleteDescription()
        {
            Description = null;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }
         
    }
}
