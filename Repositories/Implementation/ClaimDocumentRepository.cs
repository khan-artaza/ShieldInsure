using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_Insure.Repositories.Implementations
{
    public class ClaimDocumentRepository : IClaimDocumentRepository
    {
        private readonly AppDbContext _context;
        public ClaimDocumentRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<ClaimDocument>> GetAllAsync() => await _context.ClaimDocuments.ToListAsync();
        public async Task<ClaimDocument?> GetByIdAsync(int id) => await _context.ClaimDocuments.FindAsync(id);

        public async Task AddAsync(ClaimDocument document)
        {
            await _context.ClaimDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ClaimDocument document)
        {
            _context.ClaimDocuments.Update(document);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var document = await _context.ClaimDocuments.FindAsync(id);
            if (document != null)
            {
                _context.ClaimDocuments.Remove(document);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ClaimDocument>> GetDocumentsByClaimIdAsync(int claimId) =>
            await _context.ClaimDocuments.Where(d => d.ClaimId == claimId).ToListAsync();
    }
}