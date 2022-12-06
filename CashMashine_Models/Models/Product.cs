using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CashMashine_Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Цена")]
        public int Cost { get; set; }
        [DisplayName("Изображение")]
        public string Image { get; set; }
        [DisplayName("Тип")]
        public string Type { get; set; }
    }
}
