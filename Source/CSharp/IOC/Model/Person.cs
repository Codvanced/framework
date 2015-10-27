using System;
using System.Data.Linq.Mapping;
using IOC.FW.Core.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace IOC.Model
{
    [Table(Name = "Person")]
    public class Person
        : IBaseModel
    {
        [Key]
        public int IdPerson { get; set; }

        public int IdOcupation { get; set; }
        public virtual Ocupation Ocupation { get; set; }

        public string PersonName { get; set; }

        public string Gender { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Activated { get; set; }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return IdPerson.GetHashCode();
        }
    }
}