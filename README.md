# Welcome
This is a personal project building a small local cafe/bakery shop for practicing ASP .NET Core and Blazor Web App/WebAssembly.
Since this project is meant for practice and focuses on exploring the features of the technology, many features are not yet completed or carefully designed.

## Technologies Used

- .NET 9 SDK
- ASP.NET Core
- Entity Framework Core
- Blazor WebAssembly
- Microsoft Identity
- xunit
- Tailwind CSS, Bootstrap, and small components from Fluent UI for Blazor.

## Project Structure
- `BlazorEcommerce.Server`: Contains the ASP.NET Core server-side code and API controllers.
- `BlazorEcommerce.Client`: Contains the Blazor WebAssembly client-side code.
- `BlazorEcommerce.Shared`: Contains shared commons and DTOs used by both client and server.

## Getting Started

### Setup the Database

1. Update the connection string in `appsettings.json` located in the `BlazorEcommerce.Server` project.

2. Apply the migrations to create the database schema.
```
cd BlazorEcommerce.Server dotnet ef database update
```

### Run the Application

1. Open the solution in Visual Studio.
2. Set `BlazorEcommerce.Server` as the startup project.
3. Press `F5` to run the application.

### Built-in Admin User

The application seeds a default admin user with the following credentials:

- Email: `admin@yay.com`
- Password: `Admin@123`

## Features
- User authentication and authorization
- Shopping cart / Place Order
- Order management (User)
- Responsive design
- Admin panel at /Admin for CRUD operations

### TODO
- Redesign Admin pages
- Refactor code & add more unit tests 😁
- Payment integration
- Order management (Admin)
- Delivery
- Search Engine
- Etc.

## License  
This project has no license. It is released into the public domain.  
You are free to use, modify, and distribute it without restrictions.  
No warranty is provided. 
