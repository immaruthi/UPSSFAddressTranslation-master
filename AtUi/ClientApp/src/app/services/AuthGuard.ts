import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Route, Router } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router) { }



  canActivate(): boolean {
    if (localStorage.getItem("Emp_Id")) {
      return true;
    }
    else if (localStorage.getItem("Emp_Id") == null) {
      this.router.navigate(['']);
      return true;
    }
    else {
      return true;
    }
    
  }

}
