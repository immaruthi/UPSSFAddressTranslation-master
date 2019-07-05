import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Route, Router } from '@angular/router';

@Injectable()
export class LoginGuard implements CanActivate {
  constructor(private router: Router) { }
  canActivate(): boolean {
    let currentUser :any = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser == null || currentUser.token ==null) {
      return true;
    }
    else if (currentUser.token != null) {
      this.router.navigate(['/workflow']);
      console.log("in canactivate loginguard true");
      return true;
    }
    
  }

}
