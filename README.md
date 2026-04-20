# Berber Randevu - Backend

Clean Architecture yapısında .NET 8 WebAPI uygulaması.

## Proje Yapısı

```
berber_BE/
├── Domain/                  # Domain katmanı
│   ├── Entities/           # Domain modelleri
│   ├── Interfaces/         # Repository interfaces
│   └── Common/             # Ortak sınıflar (BaseEntity, etc)
│
├── Application/            # Application katmanı
│   ├── DTOs/              # Data Transfer Objects
│   ├── Services/          # Business logic services
│   └── Mappings/          # AutoMapper profilleri
│
├── Infrastructure/         # Infrastructure katmanı
│   ├── Data/              # DbContext
│   └── Repositories/      # Repository implementations
│
└── API/                    # Presentation katmanı
    ├── Controllers/       # API endpoints
    ├── Middleware/        # Custom middlewares
    ├── Program.cs         # Application startup
    └── appsettings.json   # Configuration
```

## Teknolojiler

- **.NET 8**
- **Entity Framework Core** (MySQL ile)
- **AutoMapper**
- **FluentValidation**
- **Swagger/OpenAPI**

## Başlamadan Önce

1. MySQL veritabanı çalıştığından emin olun
2. `appsettings.json` dosyasındaki connection string'i kontrol edin
3. Packages'ı restore edin: `dotnet restore`
4. Migration'ları uygulayın: `dotnet ef database update`

## Çalıştırma

```bash
dotnet run --project API
```

Swagger arayüzü: `https://localhost:5001/swagger/index.html`
