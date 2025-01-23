# Carton Caps Referral API

A .NET Core REST API service that powers the referral system for the Carton Caps mobile app, enabling users to refer friends and track referral statuses through shareable deferred deep links.

## Features

- Generate and manage referral codes
- Create shareable deferred deep links
- Track referral history
- Validate referral codes during user onboarding

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code with C# extensions

## Getting Started

1. Clone the repository:

```bash
git clone https://github.com/yourusername/carton-caps-referral-api.git
cd carton-caps-referral-api
```

2. Build the solution:

```bash
dotnet build
```

3. Run the tests:

```bash
dotnet test
```

4. Run the API:

```bash
cd src/CartonCaps.Api
dotnet run
```

The API will be available at `https://localhost:5047` by default.

## Project Structure

```
├── src/
│   └── CartonCaps.Api/           # Main API project
│       ├── Controllers/          # API endpoints
│       ├── Models/               # Request/response models
│       ├── Services/            # Business logic
│       └── Infrastructure/      # Cross-cutting concerns
├── tests/
│   └── CartonCaps.Api.Tests/    # Unit and integration tests
└── docs/
    ├── openapi.yaml            # API specification
    └── diagrams/               # Architecture diagrams
```

## API Documentation

The API is documented using OpenAPI (Swagger) specification. When running the application, you can access the Swagger UI at:

```
https://localhost:5047/swagger
```

### Key Endpoints

- `GET /referrals/my-code` - Get user's referral code
- `POST /referrals/generate-link/{shareMethod}` - Generate a shareable referral link
- `GET /referrals/history` - View referral history
- `GET /referrals/validate/{code}` - Validate a referral code

## Testing

The project includes comprehensive unit and integration tests. Run them using:

```bash
dotnet test
```
