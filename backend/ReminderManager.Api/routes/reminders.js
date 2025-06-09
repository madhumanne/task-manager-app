const express = require('express');
const router = express.Router();

// In-memory reminders store for demonstration
let reminders = [];
let idCounter = 1;

// Get all reminders
router.get('/', (req, res) => {
  res.json(reminders);
});

// Create a new reminder
router.post('/', (req, res) => {
  const { title, dueDate } = req.body;
  if (!title || !dueDate) {
    return res.status(400).json({ message: 'Title and dueDate are required.' });
  }
  const reminder = { id: idCounter++, title, dueDate, completed: false };
  reminders.push(reminder);
  res.status(201).json(reminder);
});

// Mark reminder as completed
router.post('/:id/complete', (req, res) => {
  const reminder = reminders.find(r => r.id === parseInt(req.params.id));
  if (!reminder) {
    return res.status(404).json({ message: 'Reminder not found.' });
  }
  reminder.completed = true;
  res.json(reminder);
});

// Delete a reminder
router.delete('/:id', (req, res) => {
  const index = reminders.findIndex(r => r.id === parseInt(req.params.id));
  if (index === -1) {
    return res.status(404).json({ message: 'Reminder not found.' });
  }
  reminders.splice(index, 1);
  res.status(204).send();
});

module.exports = router;
