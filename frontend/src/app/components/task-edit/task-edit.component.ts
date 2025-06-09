import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskItem } from '../../models/task-item.model';

@Component({
  selector: 'app-task-edit',
  templateUrl: './task-edit.component.html'
})
export class TaskEditComponent implements OnInit {
  taskForm: FormGroup;
  taskId: number;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: ['', Validators.required]
    });
    this.taskId = 0;
  }

  ngOnInit(): void {
    this.taskId = Number(this.route.snapshot.paramMap.get('id'));
    this.taskService.getTask(this.taskId).subscribe(task => {
      this.taskForm.patchValue(task);
    });
  }

  onSubmit(): void {
    if (this.taskForm.valid) {
      const updatedTask: TaskItem = { id: this.taskId, ...this.taskForm.value };
      this.taskService.updateTask(updatedTask).subscribe(() => {
        this.router.navigate(['/tasks']);
      });
    }
  }
}
