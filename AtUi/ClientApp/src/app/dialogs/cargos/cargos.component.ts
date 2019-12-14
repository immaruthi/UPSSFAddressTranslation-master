import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
  selector: 'app-cargos',
  templateUrl: './cargos.component.html',
  styleUrls: ['./cargos.component.css']
})
export class CargosComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data, public dialogRef: MatDialogRef<CargosComponent>) { }

  closeDialog() {
    this.dialogRef.close(false);
  }

  ngOnInit() {
  }

  getAmount(a: any, b: any) {
    return (a * b).toFixed(2);
  }

}
