using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerWebApplication.Models
{
    public class ExecutorStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int ExecutorId { get; set; }
        public ExecutorStatusCode StatusCode { get; set; }
        
        public long Date { get; set; }
    }
}