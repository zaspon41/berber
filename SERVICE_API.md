# Hizmet Yönetimi API Dokumentasyonu

## Endpoints

### 1. Hizmet Ekleme (Create Service)
**URL:** `POST /api/service/create`  
**Authentication:** ✅ Gerekli (JWT Token)  
**Request Body:**
```json
{
  "hizmetAdi": "Saç Kesim",
  "fiyat": 50
}
```
**Response (201 Created):**
```json
{
  "success": true,
  "message": "Hizmet başarıyla oluşturuldu",
  "data": {
    "id": 1,
    "hizmetAdi": "Saç Kesim",
    "fiyat": 50
  }
}
```

---

### 2. Tüm Hizmetleri Listele (Get All Services)
**URL:** `GET /api/service/list`  
**Authentication:** ❌ Opsiyonel  
**Response (200 OK):**
```json
{
  "success": true,
  "message": "2 hizmet bulundu",
  "data": [
    {
      "id": 1,
      "hizmetAdi": "Saç Kesim",
      "fiyat": 50
    },
    {
      "id": 2,
      "hizmetAdi": "Tıraş",
      "fiyat": 30
    }
  ]
}
```

---

### 3. Hizmet Detayı Getir (Get Service By ID)
**URL:** `GET /api/service/{id}`  
**Authentication:** ❌ Opsiyonel  
**Example:** `GET /api/service/1`  
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Hizmet bulundu",
  "data": {
    "id": 1,
    "hizmetAdi": "Saç Kesim",
    "fiyat": 50
  }
}
```

---

### 4. Hizmet Güncelle (Update Service)
**URL:** `PUT /api/service/update`  
**Authentication:** ✅ Gerekli (JWT Token)  
**Request Body:**
```json
{
  "id": 1,
  "hizmetAdi": "Premium Saç Kesim",
  "fiyat": 75
}
```
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Hizmet başarıyla güncellendi",
  "data": {
    "id": 1,
    "hizmetAdi": "Premium Saç Kesim",
    "fiyat": 75
  }
}
```

---

### 5. Hizmet Sil (Delete Service)
**URL:** `DELETE /api/service/delete/{id}`  
**Authentication:** ✅ Gerekli (JWT Token)  
**Example:** `DELETE /api/service/delete/1`  
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Hizmet başarıyla silindi"
}
```

---

## Database Schema

### Hizmetler Table
| Column Name | Data Type | Allow Nulls | Description |
|---|---|---|---|
| id | int | ❌ | Hizmet ID (Primary Key) |
| hizmet | nvarchar(100) | ❌ | Hizmet Adı |
| fiyat | int | ❌ | Hizmet Fiyatı (TL) |

---

## Test Örneği

1. **Admin Login Yap** (Token Al)
```bash
POST http://localhost:5000/api/auth/admin-login
Content-Type: application/json

{
  "adminUserName": "admin",
  "adminPassword": "123456"
}
```

2. **Yanıt al** (Token'ı kopyala)
```json
{
  "success": true,
  "message": "Başarıyla giriş yapıldı",
  "data": {
    "id": 1,
    "adminUserName": "admin",
    "message": "Başarıyla giriş yapıldı",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

3. **Hizmet Ekle** (Token ile)
```bash
POST http://localhost:5000/api/service/create
Content-Type: application/json
Authorization: Bearer {TOKEN_BURAYA}

{
  "hizmetAdi": "Saç Kesim",
  "fiyat": 50
}
```

---

## Error Handling

### Validation Error (400)
```json
{
  "success": false,
  "message": "Doğrulama hatası",
  "errors": [
    "Hizmet adı boş olamaz",
    "Fiyat 0'dan büyük olmalıdır"
  ]
}
```

### Unauthorized (401)
```json
{
  "success": false,
  "message": "Token geçersiz veya süresi dolmuş"
}
```

### Not Found (404)
```json
{
  "success": false,
  "message": "ID 999 ile hizmet bulunamadı"
}
```

### Server Error (500)
```json
{
  "success": false,
  "message": "Hizmet oluşturma sırasında bir hata oluştu"
}
```

---

## Validation Rules

### Hizmet Adı (hizmetAdi)
- ✅ Gerekli (Required)
- ✅ Max 100 karakter
- ❌ Boş olamaz

### Fiyat (fiyat)
- ✅ Gerekli (Required)
- ✅ 0'dan büyük olmalı
- ❌ Negatif değer olamaz

---

## Architecture

```
Domain Layer
└── Entities/Hizmet.cs

Application Layer
├── DTOs/ServiceDTOs.cs (CreateServiceRequest, ServiceResponse, UpdateServiceRequest)
├── Validators/CreateServiceValidator.cs
├── Interfaces/IServiceRepository.cs
└── Interfaces/IServiceService.cs

Infrastructure Layer
├── Repositories/ServiceRepository.cs
└── Services/ServiceService.cs

API Layer
└── Controllers/ServiceController.cs
```
