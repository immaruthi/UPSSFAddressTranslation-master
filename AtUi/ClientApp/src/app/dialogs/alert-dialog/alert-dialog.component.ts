import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
  selector: 'app-alert-dialog',
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.css']
})
export class AlertDialogComponent implements OnInit {


  constructor(@Inject(MAT_DIALOG_DATA) public data, public dialogRef: MatDialogRef<AlertDialogComponent>) { }


  closeDialog() {
    this.dialogRef.close(false);
  }


  ngOnInit() {
  }

}
