using System;
using System.ComponentModel.DataAnnotations;
using IOC.FW.Core.Abstraction.Miscellaneous;
using System.Data.Linq.Mapping;

namespace IOC.Model
{
    [Table(Name = "News")]
    public class News 
        : IPrioritySortable
    {
        [Key]
        [Column(Name = "IdNews")]
        [Display(Name = "Id da Notícia")]
        public int IdNews { get; set; }

        [Column(Name = "Title")]
        [Required(ErrorMessage = "Titulo não preenchido corretamente...")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Column(Name = "NewsDescription")]
        [Required(ErrorMessage = "Descrição não preenchida corretamente...")]
        [Display(Name = "Descrição")]
        public string NewsDescription { get; set; }

        [Column(Name = "Author")]
        [Required(ErrorMessage = "Autor não preenchido corretamente...")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Column(Name = "NewsDate")]
        [Required(ErrorMessage = "Data da Notícia não preenchida corretamente...")]
        [Display(Name = "Data da Notícia")]
        public DateTime NewsDate { get; set; }

        [Column(Name = "Created")]
        [Display(Name = "Data de Cadastro")]
        public DateTime Created { get; set; }

        [Column(Name = "Updated")]
        [Display(Name = "Data de Alteração")]
        public DateTime? Updated { get; set; }

        [Column(Name = "Priority")]
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