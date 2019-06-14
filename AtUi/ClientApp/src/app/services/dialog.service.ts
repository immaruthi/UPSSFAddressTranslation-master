import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { AlertDialogComponent } from '../dialogs/alert-dialog/alert-dialog.component';
@Injectable()
export class DialogService {

  constructor(private dialog: MatDialog) { }
  openAlertDialog(msg) {

    // this.dialog.open(AlertdialogComponent, {
    //   width: '100px',
    //   disableClose: true
    // });


    const dialogConfig = new MatDialogConfig();

    this.dialog.open(AlertDialogComponent, { width: '350px', panelClass: 'confirm-dialog-container', disableClose: true, data: { message: msg } });




  }
}
