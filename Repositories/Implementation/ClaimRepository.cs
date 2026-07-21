using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Repositories.Implementations
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly AppDbContext _context;
        public ClaimRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<Claim>> GetAllAsync() => await _context.Claims.ToListAsync();

        public async Task<Claim?> GetByIdAsync(int id) =>
            await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.ClaimDocuments)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.ClaimId == id);

        public async Task AddAsync(Claim claim)
        {
            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Claim claim)
        {
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Claim>> GetClaimsByAssignedOfficerAsync(int officerId) =>
            await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.Customer)
                .Where(c => c.AssignedOfficerId == officerId)
                .ToListAsync();

        public async Task<IEnumerable<Claim>> GetClaimsByStatusAsync(ClaimStatus status) =>
            await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.ClaimStatus == status)
                .ToListAsync();

        public async Task<IEnumerable<Claim>> GetClaimsByCustomerAsync(int customerId) =>
            await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.CustomerId == customerId)
                .ToListAsync();
    }
}