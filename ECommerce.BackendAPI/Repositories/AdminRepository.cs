using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.ParametersType;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Services;


namespace Ecommerce.BackendAPI.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;

        public AdminRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Admin?> GetAdminById(int Id)
        {
            return await _context.Admins.FindAsync(Id);
        }
    }
}
