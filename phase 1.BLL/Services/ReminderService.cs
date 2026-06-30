using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _repository;

        public ReminderService(IReminderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReminderDTO>> GetBySessionAsync(int sessionId)
        {
            var reminders = await _repository.GetBySessionIdAsync(sessionId);

            return reminders.Select(x => new ReminderDTO
            {
                Id = x.Id,
                ReminderTime = x.ScheduledAt,
                IsSent = x.Status == 2
            }).ToList();
        }

        public async Task CreateSessionRemindersAsync(Session session)
        {
            var reminderOneDay = new Reminder
            {
                SessionId = session.Id,
                Channel = 1,
                ScheduledAt = session.StartAt.AddDays(-1),
                Status = 1
            };

            var reminderTwoHours = new Reminder
            {
                SessionId = session.Id,
                Channel = 1,
                ScheduledAt = session.StartAt.AddHours(-2),
                Status = 1
            };

            await _repository.AddAsync(reminderOneDay);
            await _repository.AddAsync(reminderTwoHours);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> MarkAsSentAsync(int reminderId)
        {
            var reminder = await _repository.GetByIdAsync(reminderId);

            if (reminder == null)
                return false;

            reminder.Status = 2;
            reminder.SentAt = DateTime.UtcNow;

            _repository.Update(reminder);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task UpdateSessionRemindersAsync(Session session, DateTime oldStartAt)
        {
            if (session.StartAt == oldStartAt)
                return;

            var reminders = await _repository.GetBySessionIdAsync(session.Id);

            foreach (var reminder in reminders.Where(r => r.Status == 1))
            {
                _repository.Delete(reminder);
            }

            var reminderOneDay = new Reminder
            {
                SessionId = session.Id,
                Channel = 1,
                ScheduledAt = session.StartAt.AddDays(-1),
                Status = 1
            };

            var reminderTwoHours = new Reminder
            {
                SessionId = session.Id,
                Channel = 1,
                ScheduledAt = session.StartAt.AddHours(-2),
                Status = 1
            };

            await _repository.AddAsync(reminderOneDay);
            await _repository.AddAsync(reminderTwoHours);
            await _repository.SaveChangesAsync();
        }

        public async Task DeletePendingRemindersAsync(int sessionId)
        {
            var reminders = await _repository.GetBySessionIdAsync(sessionId);

            foreach (var reminder in reminders.Where(r => r.Status == 1))
            {
                _repository.Delete(reminder);
            }

            await _repository.SaveChangesAsync();
        }
    }
}
