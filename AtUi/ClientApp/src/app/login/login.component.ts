import { Component } from '@angular/core';
import { UserService } from '../services/UserService';
import 'rxjs/Rx';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map'; 
import { DialogService } from '../services/dialog.service';

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


  constructor(private userService: UserService, private router: Router, private formBuilder: FormBuilder, private dialogService: DialogService) {

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
      userId: ['', Validators.required],
      password: ['', Validators.required],


    });
  }
  get f() { return this.registerForm.controls; }

  onSubmit(name, password) {
    this.submitted = true;

    // stop here if form is invalid
    //if (this.registerForm.invalid) {
    //  return;
    //}


    if (name == "") {
      this.dialogService.openAlertDialog('Please Enter Username');
    }
    else if (password == "") {
      this.dialogService.openAlertDialog('Please Enter Your Password');
    }


    else {

      //check userid
      //this.userService.ValidateUserId(name)
      //  .subscribe((data: boolean) => {
      //    this.userIsExists = data;

      //    if (data) {
      this.userService.ValidateUser(name, password)
              .subscribe((data: boolean) => {
                this.isExists = data;

                if (this.isExists) {
                  //this.router.navigate(['/counter', name]);
                  this.router.navigate(['/workflow']);
                  this.setSession(name, password);


                }
                else {
                  this.dialogService.openAlertDialog('Please provide valid credentials');
                }

              });

          //}
          //else {
          //  this.dialogService.openAlertDialog('Please Check The UserName Entered');
          //}

        //});


    }





    /*

   //code check userid and password

   this.userService.ValidateUser(name, password)
     .subscribe((data: boolean) => {
       this.isExists = data;

       if (this.isExists) {
         //this.router.navigate(['/counter', name]);
         this.router.navigate(['/counter']);
         this.setSession(name, password);

         console.log(name + "in homecomponent");

       }
else{
         alert('Please check User Id and Password');
}

     });  

*/

  }


  //adding data to localstorage
  private setSession(name, password) {
    localStorage.setItem('Emp_Id', name);
    localStorage.setItem("pwd", password);
    console.log("in Setsession");
    console.log(localStorage.getItem("Emp_Id"));

  }

  private logout() {
    localStorage.removeItem("userId");
    localStorage.removeItem("password");


  }





  }


