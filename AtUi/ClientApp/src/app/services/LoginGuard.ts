import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Route, Router } from '@angular/router';

@Injectable()
export class LoginGuard implements CanActivate {
  constructor(private router: Router) { }
  canActivate(): boolean {
    if (localStorage.getItem("Emp_Id") == null) {
      return true;
    }
    else if (localStorage.getItem("Emp_Id")) {
      this.router.navigate(['/home']);
      console.log("in canactivate loginguard true");
      return true;
    }
    else {
      return true;
    }
  }

}
