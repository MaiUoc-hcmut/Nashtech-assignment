using System.ComponentModel.DataAnnotations;

namespace Ecommerce.SharedViewModel.Models
{
    public class ParentCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}