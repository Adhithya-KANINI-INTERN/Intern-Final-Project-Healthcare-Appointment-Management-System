import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-list',
  imports: [CommonModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {

  users: any[] = [];
  loading = true;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  deleteUser(email: string): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(email).subscribe(() => {
        alert('User deleted successfully');
        this.loadUsers();
      });
    }
  }
}
