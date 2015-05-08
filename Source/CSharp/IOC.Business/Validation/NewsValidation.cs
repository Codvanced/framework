using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using FluentValidation;

namespace IOC.Validation
{
    public class NewsValidation 
        : AbstractValidator<News>
    {
        public NewsValidation()
        {
            this.RuleFor(n => n.Title)
                .NotEmpty()
                .WithMessage("O título deve ser preenchido.")
                .Length(1, 150)
                .WithMessage("O título deve ter no máximo 150 caractéres.");

            this.RuleFor(n => n.Author)
                .NotEmpty()
                .WithMessage("O autor deve ser preenchido.")
                .Length(1, 100)
                .WithMessage("O autor deve ter no máximo 100 caractéres.");

            this.RuleFor(n => n.NewsDescription)
                .NotEmpty()
                .WithMessage("A descrição deve ser preenchida.");

            this.RuleFor(n => n.NewsDate)
                .NotEmpty()
                .WithMessage("A data da noticia deve ser preenchida.");
        }
    }
}
