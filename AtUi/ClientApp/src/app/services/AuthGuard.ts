import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Route, Router } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router) { }

  currentUser: any = JSON.parse(localStorage.getItem('currentUser'));

  canActivate(): boolean {
    if (this.currentUser != null && this.currentUser.token!= null) {
      return true;
    }
    else if (this.currentUser == null || this.currentUser.token == null) {
      this.router.navigate(['/login']);
      return true;
    }
    else {
      return true;
    }
    
  }

}
