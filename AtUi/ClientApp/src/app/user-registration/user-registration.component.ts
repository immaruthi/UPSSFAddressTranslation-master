import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UserService } from '../services/UserService';
import { UserReg } from '../models/UserReg';
import { DialogService } from '../services/dialog.service';
import { error } from 'util';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent implements OnInit {
  hide = true;
  userreg: UserReg;
  selected;
  cities;
  userRegForm: FormGroup;

  public hasError = (controlName: string, errorName: string) => {
    return this.userRegForm.controls[controlName].hasError(errorName);
  }

  constructor(private userservice: UserService, private dialogService: DialogService) { }

  ngOnInit() {
    this.GetAllCities();
    this.userRegForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      lastName: new FormControl('', [Validators.required, Validators.maxLength(50)]),
      email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(8)]),
      userId: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.minLength(7)]),
      cities: new FormControl('', [Validators.required]),
      role: new FormControl('', [Validators.required])
    });
  }

  onSubmit() {
    if (this.userRegForm.valid) {
      this.userreg = Object.assign({}, this.userRegForm.value);
      return this.userservice.CreateNewUser(this.userreg).subscribe(
        (result: any) => {
          this.dialogService.openAlertDialog(result);
          this.userRegForm.reset();
        }, error => { this.dialogService.openAlertDialog('Error while creating user') }
      );
    }
  }
  GetAllCities() {
    this.userservice.GetAllCities().subscribe((data) => {
      this.cities = data;
    });
  }

}
