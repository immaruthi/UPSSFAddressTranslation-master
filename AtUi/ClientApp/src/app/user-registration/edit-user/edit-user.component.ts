import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/UserService';
import { UserReg } from '../../models/UserReg';
import { DialogService } from '../../services/dialog.service';
import { MatDialog, MatDialogRef } from '@angular/material';
import { NotificationService } from '../../services/NotificationService';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {
  cities;
  userreg: UserReg;
  role: string;

  constructor(public userservice: UserService, private dialogservice: DialogService, private notificationService: NotificationService,
    private dialogref: MatDialogRef<EditUserComponent>) { }

  ngOnInit() {
    this.GetAllCities();
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.userservice.userRegForm.controls[controlName].hasError(errorName);
  }
  GetAllCities() {
    this.userservice.GetAllCities().subscribe((data) => {
      this.cities = data;
    });
  }
  onSubmit() {
    if (this.userservice.userRegeditForm.valid) {
      this.userreg = Object.assign({}, this.userservice.userRegeditForm.value);
      return this.userservice.updateUser(this.userreg).subscribe((result: any) => {
        this.notificationService.openSuccessMessageNotification(result);
        this.onClose();
        
      })
    }
  }
  onClose() {
    this.userservice.userRegeditForm.reset();
    this.dialogref.close();
  }
}
