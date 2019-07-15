import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent implements OnInit {
  hide = true;
  userRegForm: FormGroup;

  public hasError = (controlName: string, errorName: string) => {
    return this.userRegForm.controls[controlName].hasError(errorName);
  }

  constructor() { }

  ngOnInit() {
    this.userRegForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      lastName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      emailId: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      userId: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.minLength(7)]),
      city: new FormControl('', [Validators.required])
    });
  }

}
