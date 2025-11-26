using BreakFree.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakFree.BLL.Interfaces
{
    public interface IDailyStatusService
    {
        
        void AddDailyStatus(DailyStatus status);
    }
}
