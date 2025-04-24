using System.ComponentModel.DataAnnotations;

namespace Ecommerce.SharedViewModel.Models
{
    public class ParentCategory
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<Category> Categories { get; set; }
    }
}