import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-confirm-popup',
  templateUrl: './confirm-popup.component.html',
  styleUrls: ['./confirm-popup.component.scss']
})
export class ConfirmPopupComponent {
  dialogRef: any;
  
  constructor(public dialog: MatDialog) {
  }
  
  show(onConfirm, message) {
    this.dialogRef = this.dialog.open(ConfirmDialog, {
      width: '350px',
      data: {message: message, onConfirm: onConfirm}
    });
  }
  
  alert(message, type = 'info', callback: any = false) {
    this.dialogRef = this.dialog.open(AlertDialog, {
      width: '350px',
      data: {message: message, type: type, callback: callback}
    });
  }
}

@Component({
  selector: 'confirm-dialog',
  templateUrl: 'confirm-dialog.html',
})
export class ConfirmDialog {
  
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }
  
  onConfirm() {
    this.data.onConfirm();
    this.dialogRef.close();
  }
  
  onNoClick() {
    this.dialogRef.close();
  }
}


@Component({
  selector: 'alert-dialog',
  templateUrl: 'alert-dialog.html',
})
export class AlertDialog {
  
  constructor(
    public dialogRef: MatDialogRef<AlertDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }
  
  onOkClick() {
    if (this.data.callback) {
      this.data.callback();
    }
    this.dialogRef.close();
  }
  
}
