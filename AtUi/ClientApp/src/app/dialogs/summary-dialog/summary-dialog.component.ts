import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
  selector: 'app-summary-dialog',
  templateUrl: './summary-dialog.component.html',
  styleUrls: ['./summary-dialog.component.css']
})
export class SummaryDialogComponent implements OnInit {


  constructor(@Inject(MAT_DIALOG_DATA) public data, public dialogRef: MatDialogRef<SummaryDialogComponent>) { }


  closeDialog() {
    this.dialogRef.close(false);
  }


  ngOnInit() {
  }

}
