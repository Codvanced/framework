using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using IOC.FW.Core.Abstraction.Model;

namespace IOC.Model
{
    [Table("Ocupation")]
    public class Ocupation
        : IBaseModel
    {
        [Key]
        public int IdOcupation { get; set; }
        public string OcupationName { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Activated { get; set; }
    }
}