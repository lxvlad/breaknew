using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BreakFree.BLL.Interfaces;
using BreakFree.DAL.Entities;
using BreakFree.DAL.Repositories;
using System.Collections.Generic;


namespace BreakFree.BLL.Services
{
    public class DailyStatusService : IDailyStatusService
    {
        
        private readonly DailyStatusRepository _dailyStatusRepository;


        public DailyStatusService()
        {
            _dailyStatusRepository = new DailyStatusRepository();
        }


        public void AddDailyStatus(DailyStatus status)
        {
            _dailyStatusRepository.AddDailyStatus(status);
        }


        public List<DailyStatus> GetStatusesByHabit(int habitId)
        {
            return _dailyStatusRepository.GetStatusesByHabit(habitId);
        }


        public List<DailyStatus> GetStatusesByUser(int userId)
        {
            return _dailyStatusRepository.GetStatusesByUser(userId);
        }
    }
}   
