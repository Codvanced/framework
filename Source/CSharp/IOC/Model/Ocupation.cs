using System;
using IOC.FW.Core.Abstraction.Model;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace IOC.Model
{
    [Table(Name = "Ocupation")]
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