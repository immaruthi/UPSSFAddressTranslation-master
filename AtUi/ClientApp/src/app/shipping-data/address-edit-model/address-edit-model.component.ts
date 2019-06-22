import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Component, Inject } from '@angular/core';
import { ShippingService } from '../../services/shipping.service';
import { FormControl, Validators } from '@angular/forms';
import { DataService } from '../../services/data.service';
@Component({
  selector: 'app-address-edit-model',
  templateUrl: './address-edit-model.component.html',
  styleUrls: ['./address-edit-model.component.css']
})
export class AddressEditModelComponent {

  constructor(public dialogRef: MatDialogRef<AddressEditModelComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dataService: DataService) {
    debugger;
  }

  formControl = new FormControl('', [
    Validators.required
    // Validators.email,
  ]);

  getErrorMessage() {
    return this.formControl.hasError('required') ? 'Required field' :
      this.formControl.hasError('email') ? 'Not a valid email' :
        '';
  }

  submit() {
    // emppty stuff
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  stopEdit(): void {
    debugger;
    this.dataService.updateIssue(this.data);
  }
}
