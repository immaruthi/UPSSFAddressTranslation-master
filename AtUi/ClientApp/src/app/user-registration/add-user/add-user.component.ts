import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/UserService';
import { UserReg } from '../../models/UserReg';
import { DialogService } from '../../services/dialog.service';
import { MatDialogRef } from '@angular/material';
import { NotificationService } from '../../services/NotificationService';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  userreg: UserReg;
  cities;
  hide = true;
  role: string;

  constructor(public userservice: UserService, private dialogService: DialogService, private notificationService: NotificationService,
    public dialogRef: MatDialogRef<AddUserComponent>) { }

  ngOnInit() {
    this.GetAllCities();
  }

  onSubmit() {
    if (this.userservice.userRegForm.valid) {
      this.userreg = Object.assign({}, this.userservice.userRegForm.value);
      return this.userservice.CreateNewUser(this.userreg).subscribe(
        (result: any) => {
          //this.dialogService.openAlertDialog(result);
          this.notificationService.openSuccessMessageNotification(result);
          this.onClose();
        }, error => { this.notificationService.openErrorMessageNotification('Error while creating user') }
      );
    }
  }

  onClose() {
    this.userservice.userRegForm.reset();
    this.userservice.intiliazeFormGroup();
    this.dialogRef.close();
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.userservice.userRegForm.controls[controlName].hasError(errorName);
  }

  GetAllCities() {
    this.userservice.GetAllCities().subscribe((data) => {
      this.cities = data;
    });
  }
}
