using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Repositories.Implementations
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly AppDbContext _context;
        public PolicyRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Policy>> GetAllAsync() => await _context.Policies.ToListAsync();
        public async Task<Policy?> GetByIdAsync(int id) => await _context.Policies.FindAsync(id);

        public async Task AddAsync(Policy policy)
        {
            await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Policy policy)
        {
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Policy>> GetPoliciesByCustomerIdAsync(int customerId) =>
            await _context.Policies.Where(p => p.CustomerId == customerId).ToListAsync();

        public async Task<IEnumerable<Policy>> GetActivePoliciesAsync() =>
            await _context.Policies.Where(p => p.Status == PolicyStatus.Active).ToListAsync();
    }
}