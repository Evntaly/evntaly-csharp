<p align="center">
  <img src="https://cdn.evntaly.com/Resources/og.png" alt="Evntaly Cover" width="100%">
</p>

<h3 align="center">Evntaly</h3>

<p align="center">
 An advanced event tracking and analytics platform designed to help developers capture, analyze, and react to user interactions efficiently.
</p>
<p align="center">
  <a href="https://www.nuget.org/packages/EvntalySDK"><img src="https://img.shields.io/nuget/v/EvntalySDK" alt="NuGet Version"></a>
  <a href="https://github.com/Evntaly/evntaly-csharp"><img src="https://img.shields.io/github/license/Evntaly/evntaly-csharp" alt="license"></a>
</p>

# evntaly-csharp

**EvntalySDK** is a .NET client for interacting with the Evntaly event tracking platform. It provides methods to initialize tracking, log events, identify users, and check API usage limits.

## Features

- **Initialize** the SDK with a developer secret and project token.
- **Track events** with metadata and tags.
- **Identify users** for personalization and analytics.
- **Enable or disable** tracking globally.

## Installation

To install the SDK using NuGet:

```sh
 dotnet add package EvntalySDK
```

## Usage

### Initialization

Initialize the SDK with your developer secret and project token:

```csharp
using System;
using EvntalySDK;

class Program
{
    static void Main()
    {
        var evntaly = new SDK("YOUR_DEVELOPER_SECRET", "YOUR_PROJECT_TOKEN");
        Console.WriteLine("Evntaly SDK initialized!");
    }
}
```

### Tracking Events

To track an event:

```csharp
var eventData = new Event
{
    Title = "Payment Received",
    Description = "User completed a purchase",
    Message = "Order #12345",
    Data = new EventData
    {
        UserId = "67890",
        Timestamp = "2025-01-08T09:30:00Z",
        Referrer = "social_media",
        EmailVerified = true,
    },
    Tags = new string[] {"purchase", "payment", "ecommerce"},
    Notify = true,
    Icon = "💰",
    ApplyRuleOnly = false,
    User = new EventUser { ID = "12345" },
    Type = "Transaction",
    SessionID = "20750ebc-dabf-4fd4-9498-443bf30d6095_bsd",
    Feature = "Checkout",
    Topic = "@Sales"
};

await evntaly.TrackEventAsync(eventData);
```

### Identifying Users

To identify a user:

```csharp
var userData = new UserProfile
{
    ID = "12345",
    Email = "user@example.com",
    FullName = "John Doe",
    Organization = "ExampleCorp",
    Data = new UserProfileData
    {
        Id = "JohnD",
        Email = "user@example.com",
        Location = "USA",
        Salary = 75000,
        Timezone = "America/New_York",
        SubscriptionPlan = "Premium",
        LastLogin = "2025-02-24T15:30:00Z"
    }
};

await evntaly.IdentifyUserAsync(userData);
```

### Enabling/Disabling Tracking

Control event tracking globally:

```csharp
evntaly.DisableTracking();  // Disables tracking
evntaly.EnableTracking();   // Enables tracking
```

## License

This project is licensed under the MIT License.

---

*Note: Replace **`"YOUR_DEVELOPER_SECRET"`** and **`"YOUR_PROJECT_TOKEN"`** with actual credentials.*
