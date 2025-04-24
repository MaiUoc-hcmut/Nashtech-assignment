namespace Ecommerce.SharedViewModel.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<VariantCategory> VariantCategories { get; set; }
        public ParentCategory ParentCategory { get; set; }
    }
}