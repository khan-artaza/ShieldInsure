using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Repositories.Implementations
{
    public class FraudCheckRepository : IFraudCheckRepository
    {
        private readonly AppDbContext _context;
        public FraudCheckRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<FraudCheck>> GetAllAsync() => await _context.FraudChecks.ToListAsync();
        public async Task<FraudCheck?> GetByIdAsync(int id) => await _context.FraudChecks.FindAsync(id);

        public async Task AddAsync(FraudCheck fraudCheck)
        {
            await _context.FraudChecks.AddAsync(fraudCheck);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(FraudCheck fraudCheck)
        {
            _context.FraudChecks.Update(fraudCheck);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var fraudCheck = await _context.FraudChecks.FindAsync(id);
            if (fraudCheck != null)
            {
                _context.FraudChecks.Remove(fraudCheck);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FraudCheck?> GetFraudCheckByClaimIdAsync(int claimId) =>
            await _context.FraudChecks.FirstOrDefaultAsync(f => f.ClaimId == claimId);
    }
}