using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BreakFree.DAL.Entities
{
    [Table("UserSOSLogs")]
    public class UserSOSLog
    {
        [Key] 
        [Column("log_id")]
        public int LogId { get; set; }

        [Column("action_id")]
        public int ActionId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("dateTime")]
        public DateTime DateTime { get; set; }

        [Column("worked")]
        public bool Worked { get; set; }
    }
}
