using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using FluentValidation;

namespace IOC.Validation
{
    public class NoticiaValidation 
        : AbstractValidator<Noticia>
    {
        public NoticiaValidation()
        {
            RuleFor(noticia => noticia.Titulo)
                .NotEmpty()
                .WithMessage("O título deve ser preenchido.")
                .Length(1, 150)
                .WithMessage("O título deve ter no máximo 150 caractéres.");

            RuleFor(noticia => noticia.Autor)
                .NotEmpty()
                .WithMessage("O autor deve ser preenchido.")
                .Length(1, 100)
                .WithMessage("O autor deve ter no máximo 100 caractéres.");

            RuleFor(noticia => noticia.Descricao)
                .NotEmpty()
                .WithMessage("A descrição deve ser preenchida.");

            RuleFor(noticia => noticia.DataNoticia)
                .NotEmpty()
                .WithMessage("A data da noticia deve ser preenchida.");
        }
    }
}
