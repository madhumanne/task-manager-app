// Basic Express app entry point for ReminderManager.Api

const express = require('express');
const bodyParser = require('body-parser');
const remindersRouter = require('./routes/reminders');
const jwtAuth = require('./middleware/authenticate-jwt');
const cookieParser = require('cookie-parser');

const app = express();
app.use(bodyParser.json());
app.use(cookieParser());

const PORT = process.env.PORT || 4000;

// Protect all /api/reminders endpoints with JWT authentication
app.use('/api/reminders', jwtAuth, remindersRouter);

app.get('/', (req, res) => {
  res.send('Reminder Manager API is running');
});

app.listen(PORT, () => {
  console.log(`ReminderManager.Api listening on port ${PORT}`);
});
