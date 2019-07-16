import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UserService } from '../services/UserService';
import { UserReg } from '../models/UserReg';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent implements OnInit {
  hide = true;
  userreg: UserReg;
  cities;
  userRegForm: FormGroup;

  public hasError = (controlName: string, errorName: string) => {
    return this.userRegForm.controls[controlName].hasError(errorName);
  }

  constructor(private userservice: UserService) { }

  ngOnInit() {
    this.GetAllCities();
    this.userRegForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      lastName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      userId: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.minLength(7)]),
      cities: new FormControl('', [Validators.required])
    });
  }

  onSubmit() {
    if (this.userRegForm.valid) {
      this.userreg = Object.assign({}, this.userRegForm.value);
      return this.userservice.CreateNewUser(this.userreg).subscribe(
        (result: any) => {
          console.log(result);
          this.userRegForm.reset();
        });
    }
  }
  GetAllCities() {
    this.userservice.GetAllCities().subscribe((data) => {
      this.cities = data;
    });
  }

}
