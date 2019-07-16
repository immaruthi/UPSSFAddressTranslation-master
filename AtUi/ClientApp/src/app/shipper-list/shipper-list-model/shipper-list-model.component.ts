import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { FormControl, Validators } from '@angular/forms';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-shipper-list-model',
  templateUrl: './shipper-list-model.component.html',
  styleUrls: ['./shipper-list-model.component.css']
})
export class ShipperListModelComponent {

  constructor(public dialogRef: MatDialogRef<ShipperListModelComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dataService: DataService) {
  }

  formControl = new FormControl('', [
    Validators.required
    // Validators.email,
  ]);

  getErrorMessage() {
    return this.formControl.hasError('required') ? 'Required field' : '';
  }

  submit() {
    // emppty stuff
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  stopEdit(): void {
    this.dataService.updateIssue(this.data);
  }

}
