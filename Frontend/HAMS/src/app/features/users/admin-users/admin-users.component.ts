import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { User } from '../../../shared/models/user.model';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-users',
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {
  users: User[] = [];
  searchText: string = '';
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.fetchAllUsers();
  }

  fetchAllUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users = data;
      },
      error: () => {
        this.errorMessage = 'No Users Found.';
      }
    });
  }

  fetchUsersByRole(role: string): void {
    this.userService.getUsersByRole(role).subscribe({
      next: (data) => {
        this.users = data;
        this.errorMessage = null;
      },
      error: () => {
        this.errorMessage = 'Error fetching users for the selected role';
      }
    });
  }

  filterUsersByName(): void {
    if (!this.searchText) {
      this.fetchAllUsers();
      return;
    }

    this.users = this.users.filter((user) =>
      user.fullName.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }

  fetchDeleteUser(user: User): void {
    this.userService.deleteUser(user.email).subscribe({
      next: (response) => {
        this.successMessage = response;
        this.fetchAllUsers();
      },
      error: () => {
        this.errorMessage = 'Error deleting user.';
      }
    });
  }

  navigateToUpdate(user: User): void {
    this.router.navigate(['/update-user'], { queryParams: { userId: user.userId } });
  }
}
