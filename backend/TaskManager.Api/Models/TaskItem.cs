using System;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public TaskItem()
    {
        IsCompleted = false;
    }

    public void CompleteTask()
    {
        IsCompleted = true;
    }

    public void RescheduleTask(DateTime newDueDate)
    {
        DueDate = newDueDate;
    }
}