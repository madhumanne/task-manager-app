# Task Manager Application

## Overview
This project is a Task Manager application built using Angular for the frontend and .NET for the backend. It allows users to manage tasks effectively by providing functionalities to create, edit, delete, reschedule, and complete tasks. The application also implements JWT Bearer Authentication for secure access.

## Features
- User authentication using JWT Bearer tokens.
- Create, edit, delete, and complete tasks.
- Reschedule tasks with updated due dates.
- Responsive and user-friendly interface.

## Technologies Used
- **Frontend**: Angular
- **Backend**: .NET Core
- **Database**: Entity Framework Core with a SQL database
- **Authentication**: JWT Bearer Authentication

## Project Structure
```
task-manager-app
├── backend
│   ├── TaskManager.Api
│   └── README.md
└── frontend
    └── README.md
```

## Getting Started

### Prerequisites
- .NET SDK
- Node.js and npm
- Angular CLI

### Backend Setup
1. Navigate to the `backend/TaskManager.Api` directory.
2. Run `dotnet restore` to restore dependencies.
3. Update the `appsettings.json` file with your database connection string.
4. Run `dotnet run` to start the backend server.

#### Backend Changes for Login & Signup
- Migrated to the minimal hosting model using only `Program.cs` (no `Startup.cs`).
- Added `AuthController` with:
  - `POST /api/auth/signup`: Registers a new user (email, password).
  - `POST /api/auth/login`: Authenticates user and returns JWT token.
- User passwords are securely hashed before storing in the database.
- JWT Bearer Authentication is configured in `Program.cs` and `appsettings.json`.
- All protected endpoints require the JWT token in the `Authorization: Bearer <token>` header.

### Frontend Setup
1. Navigate to the `frontend` directory.
2. Run `npm install` to install dependencies.
3. Run `ng serve` to start the Angular application.
4. Open your browser and navigate to `http://localhost:4200`.

#### Frontend Features
- Login and Signup pages for user authentication.
- Task list, create, edit, delete, reschedule, and complete task UI.
- **Signup:** Form for name, email, phone, and password, submits to `/api/auth/signup`.
- **Login:** Form for email and password, submits to `/api/auth/login`.
- Authenticated users can manage their tasks.
- HTTP interceptor attaches JWT token (via cookie) to all API requests.
- Route guards restrict access to authenticated users.
- Logout clears user session and redirects to login.

### User Authentication (Login & Signup)
- To use the application, users must sign up and log in.
- **Signup:** Go to the `/signup` page in the application (e.g., `http://localhost:4200/signup`) to register a new account using your name, email, phone, and password.
- **Login:** After registering, navigate to the `/login` page (e.g., `http://localhost:4200/login`) and log in with your credentials.
- Upon successful login, a JWT token will be issued and stored (typically in local storage) for authenticating API requests.
- All subsequent API requests require the JWT token to be included in the `Authorization` header as a Bearer token.

## API Endpoints
- `POST /api/auth/signup` - Register a new user
- `POST /api/auth/login` - Authenticate user and receive JWT token
- `POST /api/tasks` - Create a new task
- `GET /api/tasks` - Retrieve all tasks
- `GET /api/tasks/{id}` - Retrieve a specific task by ID
- `PUT /api/tasks/{id}` - Update an existing task
- `DELETE /api/tasks/{id}` - Delete a task
- `POST /api/tasks/{id}/complete` - Mark a task as completed

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License
This project is licensed under the MIT License.