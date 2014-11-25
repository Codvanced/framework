using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOC.Model
{
    [Table("WT_Noticia")]
    public class Noticia
    {
        [Key]
        [Column("ID_NOTICIA")]
        [Display(Name = "Id da Notícia")]
        public int IdNoticia { get; set; }

        [Column("DSC_TITULO")]
        [Required(ErrorMessage = "Titulo não preenchido corretamente...")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Column("DSC_DESCRICAO")]
        [Required(ErrorMessage = "Descrição não preenchida corretamente...")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Column("DSC_AUTOR")]
        [Required(ErrorMessage = "Autor não preenchido corretamente...")]
        [Display(Name = "Autor")]
        public string Autor { get; set; }

        [Column("DAT_NOTICIA")]
        [Required(ErrorMessage = "Data da Notícia não preenchida corretamente...")]
        [Display(Name = "Data da Notícia")]
        public DateTime DataNoticia { get; set; }

        [Column("DAT_CADASTRO")]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DAT_ALTERACAO")]
        [Display(Name = "Data de Alteração")]
        public DateTime? DataAlteracao { get; set; }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return String.Concat(IdNoticia, Titulo, Descricao, Autor, DataNoticia).GetHashCode();
        }
    }
}
