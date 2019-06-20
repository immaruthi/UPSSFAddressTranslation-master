import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogService } from '../services/dialog.service';


@Component({
  selector: 'app-adminconfig',
  templateUrl: './adminconfig.component.html',
  styleUrls: ['./adminconfig.component.css']
})
export class AdminconfigComponent implements OnInit {

  constructor(private userService: UserService, private router: Router, private formBuilder: FormBuilder, private dialogService: DialogService) { }

  registerForm: FormGroup;
  submitted = false;
  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit(firstname, lastname, username, password) {
    
    this.submitted = true;

    // stop here if form is invalid
    //if (this.registerForm.invalid) {
    //  return;
    //}


    if (firstname == "") {
      this.dialogService.openAlertDialog('Please Enter Your First Name');
    }
    else if (password == "") {
      this.dialogService.openAlertDialog('Please Enter Your Password');
    }
    else if (lastname == "") {
      this.dialogService.openAlertDialog('Please Enter Your Last Name');
    }
    else if (username == "") {
      this.dialogService.openAlertDialog('Please Enter Your User Name');
    }


    else {

      this.userService.InsertUser(firstname, lastname, username, password).subscribe((data: boolean) => {
        this.dialogService.openAlertDialog('Data Inserted');
        this.router.navigate(['/workflow']);
      });

      //check userid
      //this.userService.ValidateUserId(name)
      //  .subscribe((data: boolean) => {
      //    //this.userIsExists = data;

      //    if (data) {
      //      this.userService.ValidateUser(name, password)
      //        .subscribe((data: boolean) => {
      //          this.isExists = data;

      //          if (this.isExists) {
      //            //this.router.navigate(['/counter', name]);
      //            this.router.navigate(['/home']);
      //            this.setSession(name, password);


      //          }
      //          else {
      //            this.dialogService.openAlertDialog('Please Check The Password Entered');
      //          }

      //        });

      //    }
      //    else {
      //      this.dialogService.openAlertDialog('Please Check The UserName Entered');
      //    }

      //  });


    }

  }
}
