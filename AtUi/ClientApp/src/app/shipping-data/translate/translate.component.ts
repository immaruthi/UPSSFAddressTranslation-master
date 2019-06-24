import { Component, ViewChild, OnInit } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatDialog, MatSnackBar, MatSnackBarConfig, MatProgressSpinner } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { DataService } from '../../services/data.service';
import { AddressEditModelComponent } from '../address-edit-model/address-edit-model.component';
import { ShipmentDetails } from '../../models/shipmentDetails';
import { Constants } from '../../shared/Constants';
import { DialogService } from '../../services/dialog.service';


@Component({
  selector: 'app-translate',
  templateUrl: './translate.component.html',
  styleUrls: ['./translate.component.css']
})

export class TranslateComponent implements OnInit {
  displayedColumns =
    ['select','actions', 'smT_STA_NR', 'smT_NR_TE', 'shP_DT', 'shP_CPY_NA', 'fsT_INV_LN_DES_TE', 'shP_ADR_TE',
      'shP_ADR_TR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD', 'imP_SLC_TE',
      'rcV_CPY_TE', 'rcV_ADR_TE', 'dsT_CTY_TE', 'dsT_PSL_TE', 'coD_TE'
    ];

  public ResponseData: any[] = [];
  public WorkflowID: any;
  public shipmentStatusList = Constants.ShipmentStatusList;
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    private router: Router, public dialog: MatDialog,
    public dataService: DataService,
    private snackBar: MatSnackBar,
    private dialogService: DialogService) {
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
      this.getTranslateData(this.WorkflowID);
    }
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

  getTranslateData(WorkflowID: any) {
    this.ResponseData = [];
    this.shippingService.getTranslateData(WorkflowID).subscribe((response: any) => {
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

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
  this.isAllSelected() ?
    this.selection.clear() :
    this.dataSource.data.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  /** Method to Translate the Data*/
  public sendForTranslate() {
    const checkedCount = this.selection.selected.length;
    if (checkedCount <= 0) {
      this.dialogService.openAlertDialog('Please select atleast one row to Translate');
    } else {
      const dataForTranslate = this.selection.selected; // Any changes can do here for sending array
      this.shippingService.sendDataForTranslate(dataForTranslate).subscribe((response: any) => {
        this.getTranslateData(this.WorkflowID); // Can change this according to the response
      }, error => (this.errorMessage = <any>error));
      console.log(dataForTranslate);
      this.selection.clear();
    }
  }

  startEdit(i: number, shipmentDetailToUpdate: any) {
    let shipmentDetails = shipmentDetailToUpdate;
    const dialogRef = this.dialog.open(AddressEditModelComponent, {
      data: {
        Id: shipmentDetailToUpdate.id,
        shP_ADR_TE: shipmentDetailToUpdate.shP_ADR_TE,
        shP_ADR_TR_TE: shipmentDetailToUpdate.shP_ADR_TR_TE,
        coD_TE: shipmentDetailToUpdate.coD_TE
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        let updatedDetails = this.dataService.getDialogData();

        shipmentDetails.shP_ADR_TE = updatedDetails.shP_ADR_TE;
        shipmentDetails.shP_ADR_TR_TE = updatedDetails.shP_ADR_TR_TE;
        shipmentDetails.coD_TE = updatedDetails.coD_TE;
        this.shippingService.UpdateShippingAddress(shipmentDetails).subscribe(response => {
          console.log(response)
          this.openSuccessMessageNotification("Data Updated Succesfully");
        },
          error => this.openErrorMessageNotification("Error while updating data"))
      }
    });
  }

  rowTranslate(i, shipmentWorkFlowRequest) {
    this.shippingService.sendDataForTranslate([shipmentWorkFlowRequest]).subscribe(
      (response:any) => {
   
        console.log(response)
        //shipmentWorkFlowRequest.shP_ADR_TR_TE = response.Shipments[0].address;
        this.openSuccessMessageNotification("Data Updated Succesfully");
        this.getTranslateData(this.WorkflowID);
    },
      error => this.openErrorMessageNotification("Error while updating data"));
  };
}
