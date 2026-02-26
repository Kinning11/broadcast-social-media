# Broadcast Social Media

A social media web application built with ASP.NET Core MVC.

This project was created to practice backend development, database relationships, authentication, and full-stack architecture using modern .NET technologies.

---

## 🚀 Features

- User registration & login (ASP.NET Identity)
- Create broadcasts (posts)
- Upload profile and broadcast images
- Like / Unlike functionality
- Follow / Unfollow users
- Trending page (Top broadcasts by likes)
- Recommended users page
- Unique username enforcement

---

## 🛠 Tech Stack

- ASP.NET Core 8 (MVC)
- Entity Framework Core
- SQL Server (LocalDB)
- ASP.NET Identity
- Bootstrap
- Custom CSS

---

## ⚙️ Run Locally

```bash
git clone https://github.com/Kinning11/broadcast-social-media.git
cd broadcast-social-media/BroadccastSocialMedia
dotnet build
dotnet ef database update
dotnet run