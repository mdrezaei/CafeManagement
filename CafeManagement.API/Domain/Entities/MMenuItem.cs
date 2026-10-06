using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagement.API.Domain.Entities
{
    public class MMenuItem
    {
        [Key]
        public Guid Id { get; private set; }
        [Required(ErrorMessage = "نام آیتم الزامی است.")]
        [MaxLength(100)]
        public string Name { get; private set; }
        [MaxLength(500)]
        public string? Description { get; private set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public string? ThumbNailUrl { get; private set; }
        public List<string>? PicturesUrl { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; private set; }

        public MMenuItem(string name, decimal price, string category)
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

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Category = category;


            IsAvailable = true;
            IsActive = true;
            IsDeleted = false;

            PicturesUrl = new List<string>();
        }


        public void SetAvailable()
        {
            if (IsDeleted && !IsActive)
            {
                throw new InvalidOperationException("برای موجود شدن ایتم ، ایتم باید فعال و حذف نشده باشد.");
            }
            IsAvailable = true;
        }

        public void SetUnavailable()
        {
            IsAvailable = false;
        }

        public void Activate()
        {
            if (IsDeleted)
            {
                throw new InvalidOperationException("برای فعال شدن ایتم ، ایتم باید حذف نشده باشد.");
            }
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            IsAvailable = false;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("اسم نمیتواند خالی باشد.");
            }

            Name = name;
        }

        public void UpdatePrice(decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("قیمت نمیتواند صفر یا کمتر از صفر باشد.");
            }

            Price = price;
        }

        public void AddDescription(string? description)
        {
            Description = description;
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
            if(picturesUrl == null)
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
            if(i == null || i < 0)
            {
                throw new ArgumentException("ورودی غیر قابل قبول برای حذف عکس.");
            }

            int? length = PicturesUrl.Count;

            if (length.HasValue && i<length)
            {
                PicturesUrl.RemoveAt(i);
            }
        }

        public void UpdateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("دسته بندی نمیتواند خالی باشد.");
            }

            Category = category;

        }



    }
}
