using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly AppDbContext _context;

        public ReminderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reminder>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.Reminders
                .Where(x => x.SessionId == sessionId)
                .OrderBy(x => x.ScheduledAt)
                .ToListAsync();
        }

        public async Task<Reminder?> GetByIdAsync(int id)
        {
            return await _context.Reminders
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Reminder>> GetDueRemindersAsync(DateTime now)
        {
            return await _context.Reminders
                .Include(x => x.Session)
                    .ThenInclude(s => s.Match)
                .Where(x => x.Status == 1 && x.ScheduledAt <= now)
                .OrderBy(x => x.ScheduledAt)
                .ToListAsync();
        }

        public async Task AddAsync(Reminder reminder)
        {
            await _context.Reminders.AddAsync(reminder);
        }

        public void Update(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Delete(Reminder reminder)
        {
            _context.Reminders.Remove(reminder);
        }
    }
}
