using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerWebApplication.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string InputType { get; set; }
        public string OutputType { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public int FirstTaskId { get; set; }
        public virtual ICollection<FlowTask> FlowTasks { get; set; }
    }
}