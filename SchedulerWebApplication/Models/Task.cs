using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;
using HotChocolate.Types;

namespace SchedulerWebApplication.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(30)]
        public string InputType { get; set; }

        [StringLength(30)]
        public string OutputType { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Command { get; set; }

        [GraphQLType(typeof(AnyType))]
        public Dictionary<string, string> DefaultEnvironmentVariables { get; set; }

        public virtual ICollection<FlowTask> FlowTasks { get; set; }
    }
}