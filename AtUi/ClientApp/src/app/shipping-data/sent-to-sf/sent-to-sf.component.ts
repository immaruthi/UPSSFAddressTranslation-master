import { Component, ViewChild, OnInit } from '@angular/core';
import { ShipmentDetails } from '../../models/shipmentdetails';
import { MatPaginator, MatTableDataSource, MatDialog, MatSnackBarConfig, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Constants } from '../../shared/Constants';
import { AddressEditModelComponent } from '../address-edit-model/address-edit-model.component';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-sent-to-sf',
  templateUrl: './sent-to-sf.component.html',
  styleUrls: ['./sent-to-sf.component.css']
})
export class SentToSfComponent implements OnInit {
  displayedColumns =
    ['actions', 'smT_STA_NR', 'smT_NR_TE', 'rcV_CPY_TE', 'rcV_ADR_TE', 'shP_ADR_TR_TE', 'shP_DT',
      'shP_CPY_NA', 'fsT_INV_LN_DES_TE', 'shP_ADR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD',
          'imP_SLC_TE', 'dsT_CTY_TE', 'dsT_PSL_TE', 'coD_TE', 'pyM_MTD', 'exP_TYP', 'spC_SLIC_NR'
    ];

  public ResponseData: any[] = [];
  public WorkflowID: any;
  public shipmentStatusList = Constants.ShipmentStatusList;
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    private router: Router, public dialog: MatDialog, public dataService: DataService, private snackBar: MatSnackBar) {
  }

  @ViewChild(MatPaginator) paginator: MatPaginator;

  /**
  * Set the paginator after the view init since this component will
  * be able to query its view for the initialized paginator.
  */
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
    this.WorkflowID = this.activatedRoute.snapshot.params.WorkflowID;
    if (this.WorkflowID) {
      this.getDataForSendToSF(this.WorkflowID);
    }
  }

  getDataForSendToSF(WorkflowID: any) {
    this.ResponseData = [];
    this.shippingService.getDataForSendToSF(WorkflowID).subscribe((response: any) => {
      this.ResponseData = response;
      this.dataSource.data = this.ResponseData;
      this.dataSource.paginator = this.paginator;
    }, error => (this.errorMessage = <any>error));
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  sendToSF() {
    // alert('Working In Progress !!');
    const dataForSendToSF = this.dataSource.data; // Any changes can do here for sending array
    this.shippingService.sendDataToSF(dataForSendToSF).subscribe((response: any) => {
      this.getDataForSendToSF(this.WorkflowID);
    }, error => (this.errorMessage = <any>error));
    console.log(dataForSendToSF);
  }

  startEdit(i: number, shipmentDetailToUpdate: any) {
    let shipmentDetails = shipmentDetailToUpdate;
    const dialogRef = this.dialog.open(AddressEditModelComponent, {
      data: {
        Id: shipmentDetailToUpdate.id,
        //shP_ADR_TE: shipmentDetailToUpdate.shP_ADR_TE,
        rcV_ADR_TE: shipmentDetailToUpdate.rcV_ADR_TE,
        shP_ADR_TR_TE: shipmentDetailToUpdate.shP_ADR_TR_TE,
        coD_TE: shipmentDetailToUpdate.coD_TE
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        let updatedDetails = this.dataService.getDialogData();

        const details = { RCV_ADR_TE: updatedDetails.rcV_ADR_TE, SHP_ADR_TR_TE: updatedDetails.shP_ADR_TR_TE, COD_TE: updatedDetails.coD_TE, WFL_ID: updatedDetails.wfL_ID, ID: updatedDetails.id }

        this.shippingService.UpdateShippingAddress(details).subscribe(response => {
          console.log(response)
          shipmentDetails.rcV_ADR_TE = updatedDetails.rcV_ADR_TE;
          shipmentDetails.shP_ADR_TR_TE = updatedDetails.shP_ADR_TR_TE;
          shipmentDetails.coD_TE = updatedDetails.coD_TE;
          shipmentDetails.wfL_ID = updatedDetails.wfL_ID;
          shipmentDetails.id = updatedDetails.id;
          this.openSuccessMessageNotification("Data Updated Succesfully");
        },
          error => this.openErrorMessageNotification("Error while updating data"))
      }
    });
  }

  openSuccessMessageNotification(message: string) {
    let config = new MatSnackBarConfig();
    this.snackBar.open(message, '',
      {
        duration: Constants.SNAKBAR_SHOW_DURATION,
        verticalPosition: "top",
        horizontalPosition: "right",
        extraClasses: 'custom-class-success'
      });
  }
  openErrorMessageNotification(message: string) {
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
