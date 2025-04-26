using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Set<Payment>().Include(p => p.Reservation).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Payment>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Payment>().Include(p => p.Reservation).Where(p => p.Reservation.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Set<Payment>().Include(p => p.Reservation).ToListAsync();
        }
        public async Task AddAsync(Payment payment)
        {
            await _context.Set<Payment>().AddAsync(payment);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Payment payment)
        {
            _context.Set<Payment>().Update(payment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var payment = await _context.Set<Payment>().FindAsync(id);
            if (payment != null)
            {
                _context.Set<Payment>().Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}