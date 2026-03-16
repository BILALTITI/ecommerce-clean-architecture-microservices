import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  isCollapsed = true;
  isUserAuthenticated = false;

  login() {
    // TODO: Implement login functionality
    console.log('Login clicked');
  }

  logout() {
    // TODO: Implement logout functionality
    console.log('Logout clicked');
  }

  getBasketCount(items: any[]): number {
    if (!items) return 0;
    return items.reduce((total, item) => total + (item.quantity || 0), 0);
  }
}
