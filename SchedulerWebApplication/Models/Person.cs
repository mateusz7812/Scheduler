using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace SchedulerWebApplication.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }


        [GraphQLIgnore]
        public virtual ICollection<Executor> Executors { get; set; }

        [GraphQLIgnore] 
        public virtual ICollection<Flow> Flows { get; set; }
    }
}