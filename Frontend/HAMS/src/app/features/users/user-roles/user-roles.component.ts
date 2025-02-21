import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { User } from '../../../shared/models/user.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-roles',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-roles.component.html',
  styleUrl: './user-roles.component.css'
})
export class UserRolesComponent {

  role: string = ''; 
  users: User[] = [];
  errorMessage: string | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {}

  fetchUsers(): void {
    if (!this.role) {
      this.errorMessage = 'Please select a role';
      return;
    }

    this.userService.getUsersByRole(this.role).subscribe({
      next: (data) => {
        this.users = data;
        this.errorMessage = null;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Error fetching users for the selected role';
      },
    });
  }
}
