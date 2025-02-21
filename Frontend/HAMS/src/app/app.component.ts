import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './core/auth.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{


  username: string | null = null;
  userRole: string | null = null;

  title = 'HAMS';

  hideNavandFooter = false;

  constructor(private authservice: AuthService, private router: Router) {}

  ngOnInit() {
    this.username = this.authservice.getFullName();
    this.userRole = this.authservice.getUserRole();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.hideNavandFooter = ['/login', '/register', '/change-password'].includes(event.urlAfterRedirects);
      }
    });
  }


  logout() {
    this.authservice.logout();

    console.log("Logging Out...");

    this.router.navigate(['/login']);
  }

}
