# 🔧 Barber Appointment System - REST API Dokumentasyonu

**Version:** 1.0.0  
**Last Updated:** May 1, 2026  
**Author:** Backend Team

---

## 📌 Base URL

```
http://192.168.1.113:5000
```

> **Development:** `http://localhost:5000`

---

## 🔐 Authentication

API'nin çoğu endpoint'i **JWT Token** gerektirir.

### **Token Alma (Admin Login)**

1. Admin login endpoint'ine POST isteği gönder
2. Response'ta `token` alanını al
3. Tüm korumalı isteklerde `Authorization` header'ına ekle

```http
Authorization: Bearer {YOUR_JWT_TOKEN}
```

---

## 📚 API Endpoints

### **1️⃣ AUTHENTICATION (Admin)**

---

#### **🔓 Admin Login**

Barber admin'i sistem'e giriş yaptırır.

```http
POST /api/auth/admin-login
Content-Type: application/json

{
  "adminUserName": "berber1",
  "adminPassword": "123456"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Başarıyla giriş yapıldı",
  "data": {
    "id": 1,
    "adminUserName": "berber1",
    "message": "Başarıyla giriş yapıldı",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6ImJlcmJlcjEiLCJpYXQiOjE2NzQwNjM5NjV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ"
  }
}
```

**Error Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Kullanıcı adı veya şifre yanlış"
}
```

---

#### **🔒 Admin Status (Protected)**

Giriş yapan admin'in bilgisini getirir. **Token Gerekli!**

```http
GET /api/auth/admin-status
Authorization: Bearer {TOKEN}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Admin bilgisi alındı",
  "data": {
    "adminId": 1,
    "adminUserName": "berber1"
  }
}
```

**Error Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Token geçersiz veya süresi dolmuş"
}
```

---

#### **🚪 Admin Logout**

Admin çıkış işlemi. Token client-side silinir (Stateless).

```http
POST /api/auth/admin-logout
Authorization: Bearer {TOKEN}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Başarıyla çıkış yapıldı"
}
```

---

### **2️⃣ SERVICE MANAGEMENT (Hizmetler)**

---

#### **➕ Hizmet Oluştur (Protected)**

Yeni hizmet ekler. **Token Gerekli!**

```http
POST /api/service/create
Content-Type: application/json
Authorization: Bearer {TOKEN}

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

**Error Response (400 Bad Request):**
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

**Validation Rules:**
- `hizmetAdi`: Required, Max 100 chars
- `fiyat`: Required, Must be > 0

---

#### **📋 Tüm Hizmetleri Listele**

Tüm hizmetleri getir. **Token İsteğe Bağlı**

```http
GET /api/service/list
```

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
      "hizmetAdi": "Sakal Tıraşı",
      "fiyat": 30
    }
  ]
}
```

---

#### **🔍 Hizmet Detayı Getir**

Tek bir hizmetin detaylarını getir. **Token İsteğe Bağlı**

```http
GET /api/service/{id}
```

**Example:**
```http
GET /api/service/1
```

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

**Error Response (404 Not Found):**
```json
{
  "success": false,
  "message": "ID 999 ile hizmet bulunamadı"
}
```

---

#### **✏️ Hizmet Güncelle (Protected)**

Hizmetin bilgisini günceller. **Token Gerekli!**

```http
PUT /api/service/update
Content-Type: application/json
Authorization: Bearer {TOKEN}

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

**Error Response (404 Not Found):**
```json
{
  "success": false,
  "message": "ID 1 ile hizmet bulunamadı"
}
```

---

#### **🗑️ Hizmet Sil (Protected)**

Hizmeti siler. **Token Gerekli!**

```http
DELETE /api/service/delete/{id}
Authorization: Bearer {TOKEN}
```

**Example:**
```http
DELETE /api/service/delete/1
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Hizmet başarıyla silindi"
}
```

**Error Response (404 Not Found):**
```json
{
  "success": false,
  "message": "ID 999 ile hizmet bulunamadı"
}
```

---

## 🧪 Test Örnekleri

### **cURL ile Test**

#### **1. Login**
```bash
curl -X POST http://localhost:5000/api/auth/admin-login \
  -H "Content-Type: application/json" \
  -d '{"adminUserName":"berber1","adminPassword":"123456"}'
```

#### **2. Hizmet Ekleme (Login'den gelen token kullan)**
```bash
curl -X POST http://localhost:5000/api/service/create \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{"hizmetAdi":"Saç Kesim","fiyat":50}'
```

#### **3. Hizmetleri Listele**
```bash
curl http://localhost:5000/api/service/list
```

#### **4. Hizmet Detayı**
```bash
curl http://localhost:5000/api/service/1
```

---

### **JavaScript (Fetch API) ile Test**

#### **Login ve Token Alma**
```javascript
async function login() {
  const response = await fetch('http://localhost:5000/api/auth/admin-login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      adminUserName: 'berber1',
      adminPassword: '123456'
    })
  });
  
  const data = await response.json();
  const token = data.data.token;
  localStorage.setItem('token', token);
  return token;
}
```

#### **Hizmet Ekleme**
```javascript
async function createService(hizmetAdi, fiyat) {
  const token = localStorage.getItem('token');
  
  const response = await fetch('http://localhost:5000/api/service/create', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({
      hizmetAdi: hizmetAdi,
      fiyat: fiyat
    })
  });
  
  return await response.json();
}
```

#### **Hizmetleri Listele**
```javascript
async function getServices() {
  const response = await fetch('http://localhost:5000/api/service/list');
  return await response.json();
}
```

---

### **Postman ile Test**

1. **New Request** oluştur
2. **POST** seç
3. URL'ye `http://localhost:5000/api/auth/admin-login` yaz
4. **Body → raw → JSON** seç
5. Aşağıdaki JSON'ı yapıştır:
```json
{
  "adminUserName": "berber1",
  "adminPassword": "123456"
}
```
6. **Send** butonuna tıkla
7. Response'tan `token`'ı kopyala
8. Yeni request'te **Headers** seçeneğine git
9. `Authorization` header'ı ekle:
   - **Key:** `Authorization`
   - **Value:** `Bearer {COPIED_TOKEN}`

---

## ⚠️ Error Codes

| Status Code | Message | Açıklama |
|---|---|---|
| **200** | OK | İstek başarılı |
| **201** | Created | Kayıt başarıyla oluşturuldu |
| **400** | Bad Request | Validation hatası veya yanlış format |
| **401** | Unauthorized | Token geçersiz, süresi dolmuş veya eksik |
| **404** | Not Found | İstenen kayıt bulunamadı |
| **500** | Internal Server Error | Server hatası |

---

## 🔄 Data Models

### **Admin Login Request**
```json
{
  "adminUserName": "string (required)",
  "adminPassword": "string (required)"
}
```

### **Service Request (Create/Update)**
```json
{
  "hizmetAdi": "string (required, max 100 chars)",
  "fiyat": "integer (required, > 0)"
}
```

### **Service Response**
```json
{
  "id": "integer",
  "hizmetAdi": "string",
  "fiyat": "integer"
}
```

### **Admin Response**
```json
{
  "id": "integer",
  "adminUserName": "string",
  "message": "string",
  "token": "string (JWT)"
}
```

### **API Response Wrapper**
```json
{
  "success": "boolean",
  "message": "string",
  "data": "object or array",
  "errors": "array (optional)"
}
```

---

## 🛡️ Security Notes

✅ **Implemented:**
- JWT Bearer Token Authentication
- CORS enabled for frontend origins
- Input validation with FluentValidation
- Password hashing ready (currently plain, consider bcrypt)
- Stateless authentication

⚠️ **Recommendations:**
- Use HTTPS in production
- Implement password hashing (bcrypt/Argon2)
- Add rate limiting
- Implement refresh token mechanism
- Add audit logging

---

## 🔌 Allowed CORS Origins

```
http://localhost:5173
http://127.0.0.1:5500
```

> Contact backend team to add more origins

---

## 📞 Support

Sorular veya sorunlar için backend team'e ulaş.

**API Server Status:** ✅ Running on `http://192.168.1.113:5000`

---

## 📋 Changelog

### Version 1.0.0 (May 1, 2026)
- ✅ Admin Authentication (Login/Logout/Status)
- ✅ Service Management (CRUD)
- ✅ JWT Bearer Token
- ✅ FluentValidation
- ✅ Error Handling
- ✅ CORS Support

---

**Generated:** May 1, 2026  
**Maintainer:** Backend Team
