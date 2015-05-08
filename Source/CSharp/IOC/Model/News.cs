using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IOC.FW.Core.Abstraction.Miscellaneous;
using System.Linq.Expressions;

namespace IOC.Model
{
    [Table("News")]
    public class News 
        : IPrioritySortable
    {
        [Key]
        [Column("IdNews")]
        [Display(Name = "Id da Notícia")]
        public int IdNews { get; set; }

        [Column("Title")]
        [Required(ErrorMessage = "Titulo não preenchido corretamente...")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Column("NewsDescription")]
        [Required(ErrorMessage = "Descrição não preenchida corretamente...")]
        [Display(Name = "Descrição")]
        public string NewsDescription { get; set; }

        [Column("Author")]
        [Required(ErrorMessage = "Autor não preenchido corretamente...")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Column("NewsDate")]
        [Required(ErrorMessage = "Data da Notícia não preenchida corretamente...")]
        [Display(Name = "Data da Notícia")]
        public DateTime NewsDate { get; set; }

        [Column("Created")]
        [Display(Name = "Data de Cadastro")]
        public DateTime Created { get; set; }

        [Column("Updated")]
        [Display(Name = "Data de Alteração")]
        public DateTime? Updated { get; set; }

        [Column("Priority")]
        public long Priority { get; set; }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return String.Concat(this.IdNews, this.Title, this.NewsDescription, this.Author, this.NewsDate).GetHashCode();
        }
    }
}