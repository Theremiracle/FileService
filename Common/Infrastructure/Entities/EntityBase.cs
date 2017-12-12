using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Entities
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string UserName { get; set; }

    }
}
