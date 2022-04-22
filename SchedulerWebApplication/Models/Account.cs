using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace SchedulerWebApplication.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        
        [GraphQLIgnore]
        public string Password { get; set; }

        public virtual ICollection<Executor> Executors { get; set; }
        public virtual ICollection<Flow> Flows { get; set; }
    }
}