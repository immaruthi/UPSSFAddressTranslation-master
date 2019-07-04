import { Injectable } from '@angular/core';
import { MatDialog, MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { Constants } from '../shared/Constants';


@Injectable()

export class NotificationService {
  constructor(private snackBar: MatSnackBar) { }

  public openSuccessMessageNotification(message: string) {
    let config = new MatSnackBarConfig();
    this.snackBar.open(message, '',
      {
        duration: Constants.SNAKBAR_SHOW_DURATION,
        verticalPosition: "top",
        horizontalPosition: "right",
        extraClasses: 'custom-class-success'
      });
  }

  public openErrorMessageNotification(message: string) {
    let config = new MatSnackBarConfig();
    this.snackBar.open(message, '',
      {
        duration: Constants.SNAKBAR_SHOW_DURATION,
        verticalPosition: "top",
        horizontalPosition: "right",
        extraClasses: 'custom-class-error'
      });
  }
}
