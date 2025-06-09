# ReminderManager.Api

A simple Node.js Express API for managing reminders.

## Endpoints

- `GET /api/reminders` - List all reminders
- `POST /api/reminders` - Create a new reminder (`title`, `dueDate` required)
- `POST /api/reminders/:id/complete` - Mark a reminder as completed
- `DELETE /api/reminders/:id` - Delete a reminder

## Usage

```sh
npm install
npm start
```

The API will run on port 4000 by default.
