# API Gateway (Ocelot)

This project is an API Gateway built with **.NET 8** and **Ocelot**. It acts as a reverse proxy, routing incoming client requests to the appropriate downstream microservices. Currently, it is configured to forward requests to the **Authentication API** for campus creation and login.

---

## 📦 Project Overview

- **Framework**: .NET 8
- **Gateway Library**: [Ocelot](https://github.com/ThreeMammals/Ocelot)
- **Base URL (Gateway)**: `http://localhost:5010` (HTTP profile)
- **Primary Downstream Service**: Authentication API (running on `https://localhost:7121` or `http://localhost:5079`)

---

## 🚀 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/) (optional)
- A downstream service (e.g., Authentication API) running and accessible.

---

## ⚙️ Configuration

The gateway behaviour is defined in `Ocelot.json`. The main sections are:

- **`Routes`** – Defines upstream request patterns and maps them to downstream endpoints.
- **`GlobalConfiguration`** – Sets the gateway’s public base URL (used for redirects and `Host` headers).

### Example Configuration

```json
{
  "Routes": [
    {
      "UpstreamPathTemplate": "/api/AuthApi/CreateCampus",
      "UpstreamHttpMethod": [ "Post" ],
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        { "Host": "localhost", "Port": 7121 }
      ],
      "DownstreamPathTemplate": "/api/AuthApi/CreateCampus"
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5010"
  }
}
Important: The BaseUrl must match the exact URL on which the gateway is listening (here http://localhost:5010). This is used internally by Ocelot to construct correct Host headers and redirect URIs.

🔧 Running the Gateway
Clone the repository (or navigate to the gateway project folder).

Restore dependencies:

bash
dotnet restore
Run the gateway (HTTP profile):

bash
dotnet run --launch-profile http
Alternatively, you can use the https profile if you have an SSL certificate, but the current configuration uses HTTP.

The gateway will start and listen on http://localhost:5010. You should see Ocelot startup logs in the console.

📬 Usage Examples
Create a Campus (POST)
Endpoint: http://localhost:5010/api/AuthApi/CreateCampus

Headers:

text
Content-Type: application/json
Request Body (JSON):

json
{
    "campusName": "MIT World Peace University",
    "campusLocation": "Pune, Maharashtra",
    "campusEmail": "mit@test.com",
    "campusPhone": "9876543210",
    "campusCode": "MIT001",
    "campusPassword": "Secure@123"
}
Expected Response:

200 OK (or appropriate status from downstream)

The gateway forwards the request to the Authentication API and returns its response.

Login (POST)
Endpoint: http://localhost:5010/api/AuthApi/Login

Headers:

text
Content-Type: application/json
Request Body (JSON):

json
{
    "identifier": "mit@test.com",
    "password": "Secure@123"
}
Expected Response:

200 OK with access token and refresh token.

🔒 SSL/TLS in Development
When downstream services use self‑signed HTTPS certificates (e.g., https://localhost:7121), .NET’s HttpClient will reject them by default. For development only, the gateway includes a bypass in Program.cs:

csharp
if (builder.Environment.IsDevelopment())
{
    ServicePointManager.ServerCertificateValidationCallback +=
        (sender, cert, chain, sslPolicyErrors) => true;
}
Do not use this in production – ensure proper certificate trust or use a trusted CA.

🛠️ Adding New Routes
To add a new downstream service, simply add a new entry in the Routes array inside Ocelot.json. For example:

json
{
  "UpstreamPathTemplate": "/api/Orders/{everything}",
  "UpstreamHttpMethod": [ "Get", "Post" ],
  "DownstreamScheme": "http",
  "DownstreamHostAndPorts": [
    { "Host": "localhost", "Port": 5001 }
  ],
  "DownstreamPathTemplate": "/api/{everything}"
}
❗ Troubleshooting
415 Unsupported Media Type
Ensure your request has Content-Type: application/json header.

Verify that the downstream controller has [ApiController] and [FromBody] attributes.

Test the downstream service directly to isolate the issue.

SSL Certificate Errors
If you see Authentication failed or SSL_ERROR, ensure the SSL bypass is active (development only).

Alternatively, change DownstreamScheme to "http" for local testing.

JSON Reader Exceptions
Check that Ocelot.json is valid JSON (no trailing commas, proper quotes).

Ocelot supports comments (//), but some JSON parsers may not; ensure the file is correctly formatted.

📄 License
This project is part of the Campus solution – refer to the main repository for licensing details.

👤 Author
Aniket Markad

Happy coding! 🚀

text
