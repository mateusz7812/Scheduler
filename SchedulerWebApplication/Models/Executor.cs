using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace SchedulerWebApplication.Models
{
    public class Executor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [GraphQLIgnore]
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual Account Account { get; set; }
    }
}