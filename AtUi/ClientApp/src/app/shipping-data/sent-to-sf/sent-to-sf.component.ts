import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ShipmentDetails } from '../../models/shipmentdetails';
import { MatPaginator, MatTableDataSource, MatDialog, MatSnackBarConfig, MatSnackBar } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Constants } from '../../shared/Constants';
import { AddressEditModelComponent } from '../address-edit-model/address-edit-model.component';
import { DataService } from '../../services/data.service';
import { DialogService } from '../../services/dialog.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-sent-to-sf',
  templateUrl: './sent-to-sf.component.html',
  styleUrls: ['./sent-to-sf.component.css']
})
export class SentToSfComponent implements OnInit {
  displayedColumns =
    ['actions', 'smT_STA_NR', 'pkG_NR_TE', 'rcV_CPY_TE', 'rcV_ADR_TE', 'shP_ADR_TR_TE', 'shP_DT',
      'shP_CPY_NA', 'fsT_INV_LN_DES_TE', 'shP_ADR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD',
          'imP_SLC_TE', 'dsT_CTY_TE', 'dsT_PSL_TE', 'coD_TE', 'pyM_MTD', 'exP_TYP', 'spC_SLIC_NR'
    ];

  private eventsSubscription: any
  @Input() events: Observable<void>;

  public ResponseData: any[] = [];
  public WorkflowID: any;
  public shipmentStatusList = Constants.ShipmentStatusList;
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    private router: Router, public dialog: MatDialog, public dataService: DataService,
    private snackBar: MatSnackBar, private dialogService: DialogService) {
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
    this.eventsSubscription = this.events.subscribe(() => {
      this.getDataForSendToSF(this.WorkflowID)
    });
  }

  ngOnDestroy() {
    this.eventsSubscription.unsubscribe()
  }

  getDataForSendToSF(WorkflowID: any) {
    this.ResponseData = [];
    this.shippingService.getDataForSendToSF(WorkflowID).subscribe((response: any) => {
      if (response!= null &&  response.success === true) {
        this.ResponseData = response.shipments;
      } else {
        this.ResponseData = [];
      }
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
    if (this.dataSource.data.length > 0) {
      const dataForSendToSF = this.dataSource.data; // Any changes can do here for sending array
      this.shippingService.sendDataToSF(dataForSendToSF).subscribe((response: any) => {
        if (response) {
          this.openSuccessMessageNotification(response);
        } else {
          this.openErrorMessageNotification("Something went wrong !!");
        }
        this.getDataForSendToSF(this.WorkflowID);
      }, error => (this.errorMessage = <any>error));
    } else {
      this.dialogService.openAlertDialog('No data to send to SF');
    }
  }

  startEdit(i: number, shipmentDetailToUpdate: any) {
    let shipmentDetails = shipmentDetailToUpdate;
    const dialogRef = this.dialog.open(AddressEditModelComponent, {
      data: {
        Id: shipmentDetailToUpdate.id,
        rcV_ADR_TE: shipmentDetailToUpdate.rcV_ADR_TE,
        shP_ADR_TR_TE: shipmentDetailToUpdate.shP_ADR_TR_TE,
        coD_TE: shipmentDetailToUpdate.coD_TE,
        pkG_NR_TE: shipmentDetailToUpdate.pkG_NR_TE,
        rcV_CPY_TE: shipmentDetailToUpdate.rcV_CPY_TE
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        let updatedDetails = this.dataService.getDialogData();

        const details = {
          SHP_ADR_TR_TE: updatedDetails.shP_ADR_TR_TE,
          COD_TE: updatedDetails.coD_TE,
          WFL_ID: shipmentDetails.wfL_ID,
          ID: shipmentDetails.id,
        }

        this.shippingService.UpdateShippingAddress(details).subscribe((response:any) => {
          console.log(response)

          shipmentDetails.shP_ADR_TR_TE = response.shipmentDataRequest.shP_ADR_TR_TE;;
          shipmentDetails.coD_TE = response.shipmentDataRequest.coD_TE;
          shipmentDetails.smT_STA_NR = response.shipmentDataRequest.smT_STA_NR;
          this.openSuccessMessageNotification("Data Updated Successfully");
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
