namespace Application.Validators;

using Application.DTOs;
using FluentValidation;

public class CreateServiceValidator : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.HizmetAdi)
            .NotEmpty().WithMessage("Hizmet adı boş olamaz")
            .MaximumLength(100).WithMessage("Hizmet adı maksimum 100 karakter olabilir");

        RuleFor(x => x.Fiyat)
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır");
    }
}

public class UpdateServiceValidator : AbstractValidator<UpdateServiceRequest>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli hizmet ID'si gereklidir");

        RuleFor(x => x.HizmetAdi)
            .NotEmpty().WithMessage("Hizmet adı boş olamaz")
            .MaximumLength(100).WithMessage("Hizmet adı maksimum 100 karakter olabilir");

        RuleFor(x => x.Fiyat)
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır");
    }
}
