import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { FormControl, Validators } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { ShipperListService } from '../../services/ShipperListService';
import { NotificationService } from '../../services/NotificationService';

@Component({
  selector: 'app-shipper-list-model',
  templateUrl: './shipper-list-model.component.html',
  styleUrls: ['./shipper-list-model.component.css']
})
export class ShipperListModelComponent {

  constructor(public dialogRef: MatDialogRef<ShipperListModelComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dataService: DataService,
    public shipperListService: ShipperListService, public notificationService: NotificationService) {
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
    //if (this.data.NEW === false) {
    //  this.data.close = '1';
    //  this.dataService.updateIssue(this.data);
    //} else {
    //  this.data.close = '0';
    // const data = {
    //    SPC_PSL_CD_TE: this.data.PostalCode,
    //    SPC_CTY_TE: this.data.City,
    //    SPC_CTR_TE: this.data.Centre,
    //    SPC_CPY_TE: this.data.ShippingCompany,
    //    SPC_NA: this.data.ShippingName,
    //    SPC_SND_PTY_CTC_TE: this.data.SendingPartyContact,
    //    SPC_ADR_TE: this.data.ShippingAddress,
    //    SPC_CTC_PH: this.data.Contact,
    //    SPC_SLIC_NR: this.data.SLICNumber,
    //    SPC_CST_ID_TE: this.data.CustomerID
    //  }
    //  this.shipperListService.addShipperData(data).subscribe((response: any) => {
    //    if (response) {
    //      if (response.success === true) {
    //        this.notificationService.openSuccessMessageNotification("Data Added Successfully.");
    //        this.dialogRef.close();
    //      } else {
    //        this.notificationService.openErrorMessageNotification(response.operatonExceptionMessage);
    //      }
    //    } else {
    //      this.notificationService.openErrorMessageNotification("Invalid exception occured, please contact administrator.");
    //    }

    //  },
    //    error => this.notificationService.openErrorMessageNotification(error.status + ' : ' + error.statusText))
    //}
  }

}
