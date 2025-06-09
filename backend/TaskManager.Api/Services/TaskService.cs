using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Models;
using TaskManager.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.Services
{
    public class TaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllTasks()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem> GetTaskById(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task<TaskItem> CreateTask(TaskItem task)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateTask(TaskItem task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskItem> CompleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return null;

            task.IsCompleted = true;
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> RescheduleTask(int id, DateTime newDueDate)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return null;

            task.DueDate = newDueDate;
            await _context.SaveChangesAsync();
            return task;
        }
    }
}