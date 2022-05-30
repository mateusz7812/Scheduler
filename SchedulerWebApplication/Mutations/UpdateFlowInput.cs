using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace SchedulerWebApplication.Models
{
    public class UpdateFlowInput
    {
        public int Id { get; set; }
        public int? FlowTaskId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}