using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreakFree.DAL.Entities
{

    [Table("DailyStatuses")]
    public class DailyStatus
    {
        [Key] 
        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("habit_id")]
        public int HabitId { get; set; }

        [Column("dateTime")]
        public DateTime DateTime { get; set; }

        [Column("trigger")]
        public string? Trigger { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("craving_level")]
        public int? CravingLevel { get; set; }
    }
}
