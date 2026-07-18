# API Gateway (Ocelot)

This project is an API Gateway built with **.NET 8** and **Ocelot**. It acts as a reverse proxy, routing incoming client requests to the appropriate downstream microservices. Currently, it is configured to forward requests to the **Authentication API** for campus creation and login.

---

## 📦 Project Overview

- **Framework:** .NET 8
- **Gateway Library:** Ocelot
- **Base URL (Gateway):** `http://localhost:5010`
- **Primary Downstream Service:** Authentication API (`https://localhost:7121` or `http://localhost:5079`)

---

## 🚀 Prerequisites

- .NET 8 SDK
- Git (optional)
- Authentication API running

---

## ⚙️ Configuration

The gateway behaviour is defined in `Ocelot.json`.

- **Routes** – Maps incoming requests to downstream services.
- **GlobalConfiguration** – Defines the gateway base URL.

### Example Configuration

```json
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/api/AuthApi/CreateCampus",
      "UpstreamHttpMethod": [ "Post" ],
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 7121
        }
      ],
      "DownstreamPathTemplate": "/api/AuthApi/CreateCampus"
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5010"
  }
}
```

> **Important:** The `BaseUrl` must match the URL on which the gateway is listening.

---

## 🔧 Running the Gateway

Restore dependencies:

```bash
dotnet restore
```

Run the gateway using the HTTP launch profile:

```bash
dotnet run --launch-profile http
```

The gateway will start on:

```
http://localhost:5010
```

---

# 📬 Usage Examples

## Create Campus

**Endpoint**

```
POST http://localhost:5010/api/AuthApi/CreateCampus
```

### Headers

```http
Content-Type: application/json
```

### Request Body

```json
{
  "campusName": "MIT World Peace University",
  "campusLocation": "Pune, Maharashtra",
  "campusEmail": "mit@test.com",
  "campusPhone": "9876543210",
  "campusCode": "MIT001",
  "campusPassword": "Secure@123"
}
```

### Expected Response

```
200 OK
```

The gateway forwards the request to the Authentication API.

---

## Login

**Endpoint**

```
POST http://localhost:5010/api/AuthApi/Login
```

### Headers

```http
Content-Type: application/json
```

### Request Body

```json
{
  "identifier": "mit@test.com",
  "password": "Secure@123"
}
```

### Expected Response

```json
{
  "accessToken": "<JWT Token>",
  "refreshToken": "<Refresh Token>"
}
```

---

# 🔒 SSL/TLS in Development

If your downstream service uses a self-signed certificate (`https://localhost:7121`), add the following code in `Program.cs`.

```csharp
if (builder.Environment.IsDevelopment())
{
    ServicePointManager.ServerCertificateValidationCallback +=
        (sender, cert, chain, sslPolicyErrors) => true;
}
```

> **Warning:** Never use this in production.

---

# 🛠️ Adding New Routes

Example:

```json
{
  "UpstreamPathTemplate": "/api/Orders/{everything}",
  "UpstreamHttpMethod": [ "Get", "Post" ],
  "DownstreamScheme": "http",
  "DownstreamHostAndPorts": [
    {
      "Host": "localhost",
      "Port": 5001
    }
  ],
  "DownstreamPathTemplate": "/api/{everything}"
}
```

---

# ❗ Troubleshooting

## 415 Unsupported Media Type

- Ensure `Content-Type: application/json` is present.
- Verify the controller uses `[ApiController]`.
- Verify request models use `[FromBody]`.
- Test the downstream API directly.

---

## SSL Certificate Errors

- Ensure the SSL bypass is enabled in development.
- Or change the `DownstreamScheme` to `http` for local testing.

---

## JSON Reader Exceptions

- Validate `Ocelot.json`.
- Remove trailing commas.
- Use double quotes for all property names.
- Ensure braces are properly closed.

---

# 📄 License

This project is part of the Campus Solution.

---

# 👤 Author

**Aniket Markad**

Happy Coding! 🚀