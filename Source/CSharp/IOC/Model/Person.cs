using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IOC.FW.Core.Abstraction.Model;
using System.Collections;

namespace IOC.Model
{
    [Table("Person")]
    public class Person
        : IBaseModel
    {
        [Key]
        public int IdPerson { get; set; }

        public int IdOcupation 
        { 
            get 
            {
                return 
                    this.Ocupation != null 
                    ? Ocupation.IdOcupation 
                    : 0; 
            } 
            set 
            {
                if (this.Ocupation == null)
                    this.Ocupation = new Ocupation();
                    
                this.Ocupation.IdOcupation = value;
            } 
        }
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
            return IdPerson.ToString().GetHashCode();
        }
    }
}