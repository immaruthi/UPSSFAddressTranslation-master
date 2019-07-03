import { Component } from '@angular/core';
import { UserService } from '../services/UserService';
import 'rxjs/Rx';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { DialogService } from '../services/dialog.service';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  isExpanded = false;
  isExists: boolean;
  userIsExists: boolean;
  public hasError = (controlName: string, errorName: string) => {
    return this.registerForm.controls[controlName].hasError(errorName);
  }


  constructor(private userService: UserService,
    private router: Router, private formBuilder: FormBuilder, private dialogService: DialogService,
    private _authService: AuthenticationService
  ) {

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  //code to test reactiveform
  registerForm: FormGroup;
  submitted = false;
  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      userId: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(10)]],
      password: ['', Validators.required],


    });
  }
  get f() { return this.registerForm.controls; }

  onSubmit(userid, password) {
    this.submitted = true;

    if (userid == "") {
      this.dialogService.openAlertDialog('Please Enter User Id');
    }
    else if (password == "") {
      this.dialogService.openAlertDialog('Please Enter Your Password');
    }
    else {

      this._authService.login(userid, password).subscribe(
        (response: any) => {
          debugger;
          if (response == true) {
            this.router.navigate(['/workflow']);
          }
          else {
            this.dialogService.openAlertDialog('Please provide valid credentials');
          }

        },
        error => {
          debugger;
          if (error.status == 401)
          this.dialogService.openAlertDialog('Please provide valid credentials');
        }
      );
    
    }
  }

  private logout() {
    this._authService.logout();
   
  }
}


