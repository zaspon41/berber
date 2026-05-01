namespace Application.Validators;

using System;
using Application.DTOs;
using FluentValidation;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.MüşteriAdı)
            .NotEmpty().WithMessage("Müşteri adı boş olamaz")
            .MaximumLength(100).WithMessage("Müşteri adı maksimum 100 karakter olabilir");

        RuleFor(x => x.MüşteriTelefon)
            .NotEmpty().WithMessage("Müşteri telefonu boş olamaz")
            .Matches(@"^\d{10}$|^\+?\d{12}$").WithMessage("Geçerli bir telefon numarası girin");

        RuleFor(x => x.HizmetId)
            .GreaterThan(0).WithMessage("Geçerli bir hizmet seçin");

        RuleFor(x => x.RandevuTarihi)
            .NotEmpty().WithMessage("Randevu tarihi boş olamaz")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Randevu tarihi bugün veya sonrası olmalıdır");

        RuleFor(x => x.RandevuSaati)
            .NotEmpty().WithMessage("Randevu saati boş olamaz");
    }
}

public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli ID gereklidir");

        RuleFor(x => x.MüşteriAdı)
            .NotEmpty().WithMessage("Müşteri adı boş olamaz")
            .MaximumLength(100).WithMessage("Müşteri adı maksimum 100 karakter olabilir");

        RuleFor(x => x.MüşteriTelefon)
            .NotEmpty().WithMessage("Müşteri telefonu boş olamaz");

        RuleFor(x => x.HizmetId)
            .GreaterThan(0).WithMessage("Geçerli bir hizmet seçin");

        RuleFor(x => x.RandevuTarihi)
            .NotEmpty().WithMessage("Randevu tarihi boş olamaz");

        RuleFor(x => x.RandevuSaati)
            .NotEmpty().WithMessage("Randevu saati boş olamaz");

        RuleFor(x => x.Durum)
            .NotEmpty().WithMessage("Durum boş olamaz")
            .Must(d => d == "Beklemede" || d == "Tamamlandı" || d == "İptal")
            .WithMessage("Durum 'Beklemede', 'Tamamlandı' veya 'İptal' olmalıdır");
    }
}
