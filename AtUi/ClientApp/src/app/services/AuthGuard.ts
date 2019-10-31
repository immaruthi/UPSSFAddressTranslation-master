import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Route, Router } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router) { }

  currentUser: any = JSON.parse(localStorage.getItem('currentUser'));

  canActivate(): boolean {
    if (this.currentUser != null && this.currentUser.token != null) {
      //let jwt = this.currentUser.token;
      //let jwtData = jwt.split('.')[1]
      //let decodedJwtJsonData = window.atob(jwtData)
      //let decodedJwtData = JSON.parse(decodedJwtJsonData);
      //let roles = decodedJwtData.roles;
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
