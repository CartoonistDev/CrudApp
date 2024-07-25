# CrudApp

CrudApp is a simple ASP.NET Core Web API application that demonstrates basic CRUD (Create, Read, Update, Delete) operations for managing user data.

## Features

- Create new users
- Retrieve a list of all users
- Update existing user information
- Delete users

## Technologies Used

- ASP.NET Core 8.0
- C#
- Microsoft SQL Server
- xUnit for unit testing
- Moq for mocking in unit tests

## Project Structure

- `CrudApp/`: Main application project
  - `Controllers/`: Contains the UsersController
  - `Models/`: Contains the User model
  - `Services/`: Contains the UserService implementation
  - `IServices/`: Contains the IUserService interface
- `CrudTest/`: Unit test project

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Microsoft SQL Server

### Setup

1. Clone the repository:
   ```bash
		  git clone https://github.com/cartoonistdev/CrudApp.git

2. Navigate to the project directory:
   ```bash
		  cd CrudApp

3. Update the connection string in `appsettings.json` to point to your SQL Server instance.

4. Run the database migrations:
   ```bash
		  dotnet ef database update

5. Run the application:
   ```bash
		  dotnet run

The API should now be running on `https://localhost:5001`.

## API Endpoints

- POST `/api/users`: Create a new user
- GET `/api/users`: Get all users
- PUT `/api/users/{id}`: Update a user
- DELETE `/api/users/{id}`: Delete a user

## Running Tests

- To run the unit tests:
  ```bash
  dotnet test

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.  
