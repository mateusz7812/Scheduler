using HotChocolate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerWebApplication.Models
{
    public class LocalAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [GraphQLIgnore]
        public string Password { get; set; }

        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}
