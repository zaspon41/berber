namespace Application.Validators;

using System;
using Application.DTOs;
using FluentValidation;

public class CreateOperatingHoursValidator : AbstractValidator<CreateOperatingHoursRequest>
{
    public CreateOperatingHoursValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .InclusiveBetween(0, 6).WithMessage("Gün 0-6 arasında olmalıdır (0=Pazar, 6=Cumartesi)");

        RuleFor(x => x.AçılışSaati)
            .NotEmpty().WithMessage("Açılış saati boş olamaz");

        RuleFor(x => x.KapanışSaati)
            .NotEmpty().WithMessage("Kapanış saati boş olamaz")
            .GreaterThan(x => x.AçılışSaati).WithMessage("Kapanış saati açılış saatinden sonra olmalıdır");
    }
}

public class UpdateOperatingHoursValidator : AbstractValidator<UpdateOperatingHoursRequest>
{
    public UpdateOperatingHoursValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli ID gereklidir");

        RuleFor(x => x.DayOfWeek)
            .InclusiveBetween(0, 6).WithMessage("Gün 0-6 arasında olmalıdır");

        RuleFor(x => x.AçılışSaati)
            .NotEmpty().WithMessage("Açılış saati boş olamaz");

        RuleFor(x => x.KapanışSaati)
            .NotEmpty().WithMessage("Kapanış saati boş olamaz")
            .GreaterThan(x => x.AçılışSaati).WithMessage("Kapanış saati açılış saatinden sonra olmalıdır");
    }
}
