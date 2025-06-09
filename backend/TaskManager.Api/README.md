# Task Manager API

This is the backend part of the Task Manager application, built using .NET. The application allows users to manage tasks with features such as creating, editing, deleting, rescheduling, and completing tasks. It also implements JWT Bearer Authentication for secure access.

## Project Structure

- **Controllers**: Contains the `TasksController` and `AuthController` for managing tasks and authentication (login/signup).
- **Models**: Contains the `TaskItem` class and user/authentication models.
- **Services**: Contains the `TaskService`, `AuthService`, and authentication/JWT services. All DB operations are performed via these services.
- **Data**: Contains the `ApplicationDbContext` class for configuring the database context using Entity Framework Core.
- **Configuration**: The `appsettings.json` file holds configuration settings including connection strings and JWT settings.
- **Program.cs**: Uses the minimal hosting model to configure services and the application's request pipeline (no `Startup.cs`).

## Features

- Create, edit, delete, reschedule, and complete tasks.
- JWT Bearer Authentication for secure API access (login & signup endpoints).
- **Session Management:** Each login creates a random session GUID stored in the database with expiry. All requests validate the session for expiry. Logout marks the session as expired.
- Entity Framework Core for database operations, always accessed through services.

## Getting Started

1. Clone the repository.
2. Navigate to the `backend` directory.
3. Restore the dependencies using `dotnet restore`.
4. Update the `appsettings.json` with your database connection string and JWT settings.
5. Run the application using `dotnet run`.

## API Endpoints

- `POST /api/auth/signup`: Register a new user.
- `POST /api/auth/login`: Authenticate user and receive JWT token.
- `POST /api/tasks`: Create a new task.
- `GET /api/tasks`: Retrieve all tasks.
- `GET /api/tasks/{id}`: Retrieve a specific task by ID.
- `PUT /api/tasks/{id}`: Update an existing task.
- `DELETE /api/tasks/{id}`: Delete a task.
- `POST /api/tasks/complete/{id}`: Mark a task as completed.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue for any suggestions or improvements.

## License

This project is licensed under the MIT License.