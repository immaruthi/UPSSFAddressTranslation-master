import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ShipmentDetails } from '../../models/shipmentdetails';
import { MatPaginator, MatTableDataSource, MatDialog, MatSnackBarConfig, MatSnackBar, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Constants } from '../../shared/Constants';
import { AddressEditModelComponent } from '../address-edit-model/address-edit-model.component';
import { DataService } from '../../services/data.service';
import { DialogService } from '../../services/dialog.service';
import { Observable } from 'rxjs';
import { ExcelService } from '../../services/ExcelExport';
import { MatStepperTab } from '../../shared/enums.service';
import { NotificationService } from '../../services/NotificationService';
import { SFErrors } from '../../shared/SFErrorCodes';


@Component({
  selector: 'app-sent-to-sf',
  templateUrl: './sent-to-sf.component.html',
  styleUrls: ['./sent-to-sf.component.css']
})
export class SentToSfComponent implements OnInit {
  displayedColumns =
    ['select', 'actions', 'wfL_ID', 'smT_STA_NR', 'pkG_NR_TE', 'rcV_CPY_TE', 'rcV_ADR_TE', 'shP_ADR_TR_TE', 'dsT_CTY_TE', 'dsT_PSL_TE',
      'csG_CTC_TE', 'pH_NR', 'fsT_INV_LN_DES_TE', 'shP_CPY_NA', 'shP_ADR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD',
      'imP_SLC_TE', 'coD_TE', 'poD_RTN_SVC', 'pyM_MTD', 'exP_TYP', 'spC_SLIC_NR', 'spC_CST_ID_TE'
    ];

  private eventsSubscription: any
  @Input() events: Observable<void>;

  public ResponseData: any[] = [];
  public WorkflowID: any;
  public shipmentStatusList = Constants.ShipmentStatusList;
  public PODoptions = Constants.PODoptions;
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);
  public mainData: any[] = [];
  public checkedData: any[] = [];
  public tableData: any[] = [];
  public excelMainData: any[] = [];
  filterText: string = '';
  toggleSelectAll: string = 'Select All';

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    private router: Router, public dialog: MatDialog, public dataService: DataService,
    private snackBar: MatSnackBar, private dialogService: DialogService,
    private excelService: ExcelService,
    private notificationService: NotificationService) {
  }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  /**
  * Set the paginator after the view init since this component will
  * be able to query its view for the initialized paginator.
  */
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnInit() {
    this.WorkflowID = this.activatedRoute.snapshot.params.WorkflowID;
    this.eventsSubscription = this.events.subscribe((event: any) => {
      let selectedTabIndex = event.selectedIndex;
      if (this.WorkflowID && selectedTabIndex == MatStepperTab.SendToSFTab) {
        this.getDataForSendToSF(this.WorkflowID);
      }
     
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
      this.dataSource.sort = this.sort;
      this.selection.clear();
      this.filterText = '';
      this.applyFilter('');
      this.toggleSelectAll = 'Select All';
    }, error => (this.errorMessage = <any>error));
  }

  getValidData(dataArray: any[]) {
    return dataArray.filter(data => (data.smT_STA_NR !== 3));
  }

  applyFilter(filterValue: string) {
    this.filterText = filterValue;
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
    this.selection.clear();
    this.toggleSelectAll = 'Select All';
  }

  isAllSelected() {
    const currentData = this.dataSource._orderData(this.dataSource.filteredData);
    const MainData: any[] = this.dataSource._pageData(currentData);
    const ValidData: any[] = this.getValidData(MainData);
    const checkedDataCount = ValidData.length;
    var count: number = 0;
    ValidData.forEach(row => {
      if (this.selection.isSelected(row)) {
        count = count + 1;
      }
    });

    return checkedDataCount === count;
  }

  masterToggle() {
    this.mainData = [];
    this.checkedData = [];
    //this.dataSource.data.forEach(row => this.mainData.push(row));
    const currentData = this.dataSource._orderData(this.dataSource.filteredData);
    this.mainData = this.dataSource._pageData(currentData);
    this.checkedData = this.getValidData(this.mainData);
    this.isAllSelected() ? this.AllSelectedTrue() : this.AllSelectionFalse();
  }

  AllSelectedTrue() {
    //this.selection.clear()
    this.checkedData.forEach(row => this.selection.deselect(row));
  }

  AllSelectionFalse() {
    //this.selection.clear(),
    this.checkedData.forEach(row => this.selection.select(row));
  }

  toggleSelect() {
    if (this.toggleSelectAll === 'Select All') {
      this.selection.clear();
      const dataSourceData: any[] = this.dataSource.filteredData;
      const mainDataAll = this.getValidData(dataSourceData);
      mainDataAll.forEach(row => this.selection.select(row));
      this.toggleSelectAll = 'Deselect All'
    } else {
      this.selection.clear();
      this.toggleSelectAll = 'Select All'
    }
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  sendToSF() {
    const checkedCount = this.selection.selected.length;
    if (checkedCount <= 0) {
      this.dialogService.openAlertDialog('Please select minimum one row to send to SF.');
    } else {
      const dataForSendToSF = this.selection.selected; // Any changes can do here for sending array
      this.shippingService.sendDataToSF(dataForSendToSF).subscribe((response: any) => {
        if (response.response === true) {
          const SuccessCount = response.processedShipments.length;
          const SuccessList = response.processedShipments;
          const FailedCount = response.failedToProcessShipments.length;
          const FailedList = response.failedToProcessShipments;

          var FailedMainList = [];
          for (let List of FailedList) {
            const FailedDetails = List.split(":", 3);
            const PKGNR = FailedDetails[0];
            const ErrorCode = FailedDetails[1];
            const ErrorEN = SFErrors[ErrorCode] ? SFErrors[ErrorCode] : 'Unspecified Error';
            const ErrorCH = FailedDetails[2];
            //FailedMainList.push(PKGNR + ' ' + ErrorCode + ' ' + ErrorEN);
            FailedMainList.push({ 'PKGNR': PKGNR, 'ErrorCode': ErrorCode, 'ErrorEN': ErrorEN, 'ErrorCH': ErrorCH});
          }

          const data = {
            successCount: SuccessCount,
            successList: SuccessList,
            failedCount: FailedCount,
            failedList: FailedMainList,
            screenFrom: 'SendToSF'
          }
          if (response.processedShipments.length > 0) {
            this.getDataForSendToSF(this.WorkflowID);
          }
          this.selection.clear();
          this.dialogService.openSummaryDialog(data);
        }
      }, error =>
        this.notificationService.openErrorMessageNotification(error.status + ' : ' + error.statusText)
      );
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
        rcV_CPY_TE: shipmentDetailToUpdate.rcV_CPY_TE,
        poD_RTN_SVC: shipmentDetailToUpdate.poD_RTN_SVC,
        is__ADR_TR_TE_Required: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        let updatedDetails = this.dataService.getDialogData();

        if (updatedDetails.coD_TE == shipmentDetailToUpdate.coD_TE
          && updatedDetails.shP_ADR_TR_TE.toLowerCase() == shipmentDetailToUpdate.shP_ADR_TR_TE.toLowerCase()
          && updatedDetails.poD_RTN_SVC == shipmentDetailToUpdate.poD_RTN_SVC) {

          this.notificationService.openSuccessMessageNotification("No changes found to update");
          return;
        }

        const details = {
          SHP_ADR_TR_TE: updatedDetails.shP_ADR_TR_TE.trim(),
          COD_TE: updatedDetails.coD_TE,
          WFL_ID: shipmentDetails.wfL_ID,
          ID: shipmentDetails.id,
          POD_RTN_SVC: updatedDetails.poD_RTN_SVC,
          RCV_ADR_TE: updatedDetails.rcV_ADR_TE
        }

        this.shippingService.UpdateShippingAddress(details).subscribe((response:any) => {
          console.log(response)

          //shipmentDetailToUpdate.shP_ADR_TR_TE = response.shipmentDataRequest.shP_ADR_TR_TE;;
          //shipmentDetailToUpdate.coD_TE = response.shipmentDataRequest.coD_TE;
          //shipmentDetailToUpdate.smT_STA_NR = response.shipmentDataRequest.smT_STA_NR;
          //shipmentDetailToUpdate.smT_STA_TE = response.shipmentDataRequest.smT_STA_TE;
          //shipmentDetailToUpdate.poD_RTN_SVC = response.shipmentDataRequest.poD_RTN_SVC;
          this.getDataForSendToSF(this.WorkflowID);
          this.notificationService.openSuccessMessageNotification("Data Updated Successfully.");
        },
          error => this.notificationService.openErrorMessageNotification(error.status + ' : ' + error.statusText))
      }
    });
  }

  SFexportToExcel() {
    this.tableData = [];
    this.excelMainData = [];
    this.tableData = this.dataSource.sortData(this.dataSource.filteredData, this.dataSource.sort);
    if (this.tableData.length > 0) {
      for (let data of this.tableData) {
        this.excelMainData.push(
          {
            'Workflow ID': data.wfL_ID,
            'SHP Status': this.shipmentStatusList[data.smT_STA_NR === null ? 4 : data.smT_STA_NR].value,
            'Package Number': data.pkG_NR_TE,
            'Receiving Company': data.rcV_CPY_TE,
            'Receiving Address': data.rcV_ADR_TE,
            'Translated Address': data.shP_ADR_TR_TE,
            'Receiving City': data.dsT_CTY_TE,
            'Receiving Postal Code': data.dsT_PSL_TE,
            'Consignee Contact': data.csG_CTC_TE,
            'Consignee Phone': data.pH_NR,
            'Specification': data.fsT_INV_LN_DES_TE,
            'SHP Company Name': data.shP_CPY_NA,
            'SHP Address': data.shP_ADR_TE,
            'SHP Contact': data.shP_CTC_TE,
            'SHP Phone': data.shP_PH_TE,
            'Origin City': data.orG_CTY_TE,
            'Origin Postal code': data.orG_PSL_CD,
            'IMP SLC': data.imP_SLC_TE,
            'COD': data.coD_TE,
            'Extra Service': this.PODoptions[data.poD_RTN_SVC === null ? 0 : data.poD_RTN_SVC].value,
            'Payment Method': data.pyM_MTD,
            'Express Type': data.exP_TYP,
            'Slic': data.spC_SLIC_NR
          })
      }
      this.excelService.exportAsExcelFile(this.excelMainData, 'SendToSF');
    } else {
      this.dialogService.openAlertDialog('No data for export.');
    }    
  }

  rowDelete(index: number, rowData: any) {

  }

  delete() {
    const checkedCount = this.selection.selected.length;
    if (checkedCount <= 0) {
      this.dialogService.openAlertDialog('Please select minimum one row to Delete.');
    } else {
      const dialogRef = this.dialogService.openConfirmationDialog('Are you sure, you want to delete all the selected records ?');

      dialogRef.afterClosed().subscribe(data => {
        if (data === true) {
          const dataForDelete = this.selection.selected; // Any changes can do here for sending array
          this.deleteData(dataForDelete);
        } else {

        }
      })
    }
  }

  deleteData(data: any) {
    this.shippingService.deleteUploadedData(data).subscribe((response: any) => {
      if (response != null && response.success === true) {
        if (response.hasWorkflow === false) {
          this.router.navigate(['/workflow']);
        } else {
          this.getDataForSendToSF(this.WorkflowID);
        }
        this.notificationService.openSuccessMessageNotification("Deleted Successfully");
      } else {
        this.notificationService.openErrorMessageNotification("Invalid exception occured, please contact administrator.");
      }
    },
      error => this.notificationService.openErrorMessageNotification(error.status + ' : ' + error.statusText));
  }
}
