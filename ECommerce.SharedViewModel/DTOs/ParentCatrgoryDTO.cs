namespace Ecommerce.SharedViewModel.DTOs
{
    public class ParentCategoryDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}