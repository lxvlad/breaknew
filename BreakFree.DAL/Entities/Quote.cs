using System.ComponentModel.DataAnnotations.Schema;

namespace BreakFree.DAL.Entities
{
    [Table("Quotes")]
    public class Quote
    {
        [Column("quote_id")]
        public int QuoteId { get; set; }

        [Column("text")]
        public string Text { get; set; } = string.Empty;
    }
}
