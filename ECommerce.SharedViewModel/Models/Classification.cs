using System.ComponentModel.DataAnnotations;

namespace Ecommerce.SharedViewModel.Models
{
    public class Classification
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<ProductClassification> ProductClassifications {get; set; }
    }
}