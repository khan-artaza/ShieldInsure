using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Repositories.Implementations
{
    public class SettlementLogRepository : ISettlementLogRepository
    {
        private readonly AppDbContext _context;
        public SettlementLogRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<SettlementLog>> GetAllAsync() => await _context.SettlementLogs.ToListAsync();
        public async Task<SettlementLog?> GetByIdAsync(int id) => await _context.SettlementLogs.FindAsync(id);

        public async Task AddAsync(SettlementLog settlementLog)
        {
            await _context.SettlementLogs.AddAsync(settlementLog);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(SettlementLog settlementLog)
        {
            _context.SettlementLogs.Update(settlementLog);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var log = await _context.SettlementLogs.FindAsync(id);
            if (log != null)
            {
                _context.SettlementLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SettlementLog?> GetSettlementByClaimIdAsync(int claimId) =>
            await _context.SettlementLogs.FirstOrDefaultAsync(s => s.ClaimId == claimId);
    }
}