# 💈 Barber Appointment System - Backend API

Türk berber dükkanı için tam kapsamlı randevu yönetim sistemi. .NET 8 WebAPI ile geliştirilmiş, production-ready backend.

**Dil:** Türkçe | **Framework:** .NET 8 | **Database:** SQL Server 2019 | **Auth:** JWT

---

## 🚀 Quick Start

### Gereksinimler
- .NET 8 SDK
- SQL Server 2019 (Windows Auth)
- Visual Studio 2022 veya VS Code

### Kurulum

```bash
# Repository clone et
git clone https://github.com/zaspon41/berber.git
cd berber_BE

# Build et
dotnet build

# Database migration (ilk kez)
cd API
dotnet ef database update

# API'yı çalıştır
dotnet run
```

API şu adrestte çalışacak: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

---

## 📊 Veritabanı Schema

| Tablo | Amaç |
|-------|------|
| **Admin** | Admin kullanıcıları, JWT authentication |
| **Hizmet** | Berber hizmetleri (saç kesimi, tıraş vb) |
| **OperatingHours** | Çalışma saatleri (Pazartesi-Pazar) |
| **BlockedDates** | Kapalı günler (tatil, hastalık) |
| **Appointment** | Müşteri randevuları |

---

## 🏗️ Proje Mimarisi

```
berber_BE/
├── Domain/                    # Entity modeller
│   └── Entities/
│       ├── Admin.cs
│       ├── Hizmet.cs
│       ├── OperatingHours.cs
│       ├── BlockedDates.cs
│       └── Appointment.cs
│
├── Application/               # Business logic
│   ├── DTOs/                  # Data transfer objects
│   ├── Interfaces/            # Repository & Service interfaces
│   ├── Services/              # Business logic implementation
│   ├── Validators/            # FluentValidation rules
│   ├── Mappings/              # AutoMapper profiles
│   └── Exceptions/            # Custom exceptions
│
├── Infrastructure/            # Data access
│   ├── Repositories/          # EF Core implementations
│   └── Data/
│       └── ApplicationDbContext.cs
│
└── API/                       # REST endpoints
    ├── Controllers/           # API controllers
    ├── Program.cs            # Dependency injection
    └── appsettings.json      # Configuration
```

**Mimari:** Clean Architecture (4 katman)

---

## 🔌 API Endpoints

### 1. **Authentication** (`/api/Auth`)

```http
POST /admin-login
Content-Type: application/json

{
  "email": "admin@barber.com",
  "password": "Password123"
}

Response: { "success": true, "data": { "token": "jwt_token_here" } }
```

| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| POST | `/admin-login` | None | Admin JWT token al |
| POST | `/admin-logout` | JWT | Çıkış yap |
| GET | `/admin-status` | JWT | Admin kontrolü |

---

### 2. **Services** (`/api/Service`)

| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| POST | `/create` | JWT | Yeni hizmet ekle |
| GET | `/list` | None | Tüm hizmetleri listele |
| GET | `/{id}` | None | Hizmet detayı |
| PUT | `/update` | JWT | Hizmet güncelle |
| DELETE | `/delete/{id}` | JWT | Hizmet sil |

**Örnek Request:**
```json
{
  "hizmetAdi": "Saç Kesimi",
  "fiyat": 100
}
```

---

### 3. **Operating Hours** (`/api/OperatingHours`)

| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| POST | `/create` | JWT | Çalışma saati ekle |
| GET | `/list` | JWT | Tüm saatleri listele |
| GET | `/{id}` | JWT | Saat detayı |
| PUT | `/update` | JWT | Saat güncelle |
| DELETE | `/delete/{id}` | JWT | Saat sil |
| GET | `/available/{date}` | None | Verilen tarihte müsait saatler |

**DayOfWeek:** 0=Pazartesi, 1=Salı, ..., 6=Pazar

---

### 4. **Blocked Dates** (`/api/BlockedDates`)

| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| POST | `/create` | JWT | Kapalı gün ekle |
| GET | `/list` | JWT | Tüm kapalı günleri listele |
| GET | `/{id}` | JWT | Detay |
| PUT | `/update` | JWT | Güncelle |
| DELETE | `/delete/{id}` | JWT | Sil |
| GET | `/month/{year}/{month}` | None | Ay içindeki kapalı günler |

---

### 5. **Appointments** (`/api/Appointment`)

| Method | Endpoint | Auth | Açıklama |
|--------|----------|------|----------|
| POST | `/create` | JWT | Admin tarafından randevu oluştur |
| GET | `/list` | JWT | Tüm randevuları listele |
| GET | `/{id}` | JWT | Randevu detayı |
| PUT | `/update` | JWT | Randevu güncelle |
| DELETE | `/delete/{id}` | JWT | Randevu sil |
| **POST** | **`/book`** | **None** | **Müşteri randevu al** ⭐ |
| **PUT** | **`/cancel/{id}`** | **None** | **Müşteri randevu iptal et** ⭐ |

**Public Endpoints (Müşteri tarafı):**

```http
POST /api/Appointment/book
Content-Type: application/json

{
  "müşteriAdı": "Ahmet Yıldız",
  "müşteriTelefon": "05551234567",
  "hizmetId": 1,
  "randevuTarihi": "2026-05-10T14:30:00",
  "randevuSaati": "02:30:00",
  "notlar": "Kısa kesim istiyorum"
}
```

```http
PUT /api/Appointment/cancel/1
Content-Type: application/json
```

---

## 🔐 Authentication

**JWT Bearer Token:**
- Süresi: 24 saat
- Secret: `appsettings.json` içinde
- Stateless design

**Header kullan:**
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## ✅ Özellikler

- ✅ Tam JWT authentication
- ✅ 20 API endpoint (Admin + Customer)
- ✅ Hizmet yönetimi (saatlik, fiyatlı)
- ✅ Çalışma saatleri yönetimi
- ✅ Kapalı günler yönetimi
- ✅ Müşteri randevu booking
- ✅ Müşteri randevu iptal
- ✅ Türkçe validasyon mesajları
- ✅ Comprehensive error handling
- ✅ Swagger/OpenAPI documentation
- ✅ Clean Architecture
- ✅ Entity Framework Core 8.0.3
- ✅ FluentValidation
- ✅ AutoMapper

---

## 🚧 Yapılacaklar (Backlog)

- [ ] SMS OTP doğrulama (Netgsm/Twilio)
- [ ] Randevu hatırlatması (24h, 1h öncesi)
- [ ] Hangfire background jobs
- [ ] Email bildirimleri
- [ ] Docker containerization
- [ ] Unit & Integration Tests
- [ ] Rate limiting
- [ ] Logging (Serilog)

---

## 📝 Teknoloji Stack

| Kategori | Teknoloji |
|----------|-----------|
| Runtime | .NET 8 |
| Language | C# 12 (nullable reference types) |
| Database | SQL Server 2019 |
| ORM | Entity Framework Core 8.0.3 |
| Auth | JWT Bearer |
| Validation | FluentValidation 11.9.2 |
| Mapping | AutoMapper 13.0.1 |
| API Docs | Swagger/OpenAPI |
| Architecture | Clean Architecture |

---

## 🔧 Configuration

**appsettings.json:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=192.168.1.113;Database=BerberDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-min-32-characters",
    "Issuer": "BerberAPI",
    "Audience": "BerberApp",
    "ExpiryMinutes": 1440
  }
}
```

---

## 📚 API Testing

**Swagger UI:** http://localhost:5000/swagger

**Test Tools:**
- Postman
- Insomnia
- VS Code REST Client

---

## 🐛 Error Handling

Tüm endpoint'ler standardized `ApiResponse<T>` döndürür:

```json
{
  "success": true,
  "message": "İşlem başarılı",
  "data": { /* ... */ },
  "errors": null
}
```

**Error Response:**
```json
{
  "success": false,
  "message": "Lütfen bilgilerinizi kontrol ediniz",
  "data": null,
  "errors": [
    "Email geçersiz",
    "Şifre en az 8 karakter olmalıdır"
  ]
}
```

---

## 📄 Lisans

MIT License

---

## 👨‍💻 Geliştirici

Berber Randevu Sistemi - .NET Backend

**Frontend:** Ayrıca geliştirilecek (React/Angular/Vue)

---

## 📞 İletişim

GitHub: https://github.com/zaspon41/berber

---

## 🎯 Build Status

- ✅ 0 Hata
- ✅ Production Ready
- ✅ Swagger Test Passed
- ✅ API Endpoints: 20

---

**Son Güncelleme:** 1 Mayıs 2026  
**Status:** ✅ Production Ready

Swagger arayüzü: `https://localhost:5001/swagger/index.html`
