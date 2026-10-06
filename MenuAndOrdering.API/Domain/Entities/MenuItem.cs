using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAndOrdering.API.Domain.Entities
{
    public class MenuItem
    {
        [Key]
        public Guid Id { get; private set; }
        [Required(ErrorMessage = "نام آیتم الزامی است.")]
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(500)]
        public string? Description { get; private set; }
        [Required]
        [Column(TypeName ="decimal(18,0)")]
        public decimal Price { get; private set; }
        public bool IsAvailable { get; private set; } 
        public bool IsActive { get; private set; } 
        public bool IsDeleted { get; private set; }
        public string? ThumbNailUrl { get; private set; }
        public List<string>? PicturesUrl { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; private set; }

        public MenuItem(string name, string category, decimal price, Guid id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if (price <= 0)
            {
                throw new ArgumentException("قیمت نمیتواند صفر یا کمتر از صفر باشد.");
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("دسته بندی نمیتواند خالی باشد.");
            }

            Id = id;
            Name = name;
            Category = category;
            Price = price;

            IsActive = true;
            IsAvailable = true;
            IsDeleted = false;

            PicturesUrl = new List<string>();

        }

        public MenuItem (string name, string category, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if(price <= 0)
            {
                throw new ArgumentException("قیمت نمیتواند صفر یا کمتر از صفر باشد.");
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("دسته بندی نمیتواند خالی باشد.");
            }

            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Price = price;

            IsActive = true;
            IsAvailable = true;
            IsDeleted = false;

            PicturesUrl = new List<string> ();

        }

        public void UpdatePrice (decimal newPrice)
        {
            if(newPrice <= 0)
            {
                throw new ArgumentException("قیمت نمیتواند کمتر یا مساوی صفر باشد.");
            }
            Price = newPrice;
        }

        public void SetUnavailable()
        {
            IsAvailable = false;
        }

        public void SetAvailable()
        {
            if (IsDeleted && !IsActive)
            {
                throw new InvalidOperationException("برای موجود شدن ایتم ، ایتم باید فعال و حذف نشده باشد.");
            }

            IsAvailable = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            if (IsDeleted)
            {
                throw new InvalidOperationException("ایتم حذف شده فعال نمیتواند بشود.");
            }

            IsActive = true;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }

            Name = name;
        }

        public void UpdateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("دسته بندی نمیتواند خالی باشد.");
            }

            Category = category;

        }

        public void SoftDelete()
        {
            IsDeleted = true;
            IsAvailable = false;
            IsActive = false;
        }

        public void UpdateInfo(string name, string? description, string category, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }
            if (price <= 0)
            {
                throw new ArgumentException("قیمت نمیتواند صفر یا کمتر از صفر باشد.");
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("دسته بندی نمیتواند خالی باشد.");
            }

            Name = name;
            Category = category;
            Description = description;
            Price = price;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }

        public void DeleteDescription()
        {
            Description = null;
        }

        public void AddThumbNailUrl(string? thumbNailUrl)
        {
            ThumbNailUrl = thumbNailUrl;

            if (!string.IsNullOrWhiteSpace(thumbNailUrl))
            {
                PicturesUrl.Add(thumbNailUrl);
            }
        }

        public void SetThumbNail(List<string>? picturesUrl, int i)
        {
            if (picturesUrl == null)
            {
                throw new ArgumentException("عکسی وجود ندارد.");
            }

            ThumbNailUrl = null;
            ThumbNailUrl = PicturesUrl[i];
        }

        public void UpdateThumbNailUrl(string? thumbNailUrl)
        {
            if (!string.IsNullOrWhiteSpace(ThumbNailUrl))
            {
                PicturesUrl.Remove(ThumbNailUrl);
            }

            ThumbNailUrl = thumbNailUrl;

            if (!string.IsNullOrWhiteSpace(thumbNailUrl))
            {
                PicturesUrl.Add(thumbNailUrl);
            }

        }

        public void DeleteThumbNailUrl()
        {
            if (!string.IsNullOrWhiteSpace(ThumbNailUrl))
            {
                PicturesUrl.Remove(ThumbNailUrl);
            }

            ThumbNailUrl = null;

        }

        public void AddPicturesUrl(List<string>? picturesUrl)
        {
            if (picturesUrl == null)
            {
                throw new ArgumentNullException("هیچ عکسی وجود ندارد.");
            }

            foreach (string pUrl in picturesUrl)
            {
                if (!string.IsNullOrWhiteSpace(pUrl))
                {
                    PicturesUrl.Add(pUrl);
                }
            }
        }

        public void UpdatePicturesUrl(string pUrl, int? i = null)
        {
            if (i != null)
            {
                PicturesUrl.RemoveAt(i.Value);
                if (!string.IsNullOrWhiteSpace(pUrl))
                {
                    PicturesUrl.Add(pUrl);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(pUrl))
                {
                    PicturesUrl.Add(pUrl);
                }
            }
        }

        public void DeletePicturesUrl(int i)
        {
            if (i == null || i < 0)
            {
                throw new ArgumentException("ورودی غیر قابل قبول برای حذف عکس.");
            }

            int? length = PicturesUrl.Count;

            if (length.HasValue && i < length)
            {
                PicturesUrl.RemoveAt(i);
            }
        }

    }
}
