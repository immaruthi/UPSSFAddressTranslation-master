import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatDialog, MatSnackBar, MatSnackBarConfig, MatProgressSpinner, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { DataService } from '../../services/data.service';
import { AddressEditModelComponent } from '../address-edit-model/address-edit-model.component';
import { ShipmentDetails } from '../../models/shipmentDetails';
import { Constants } from '../../shared/Constants';
import { DialogService } from '../../services/dialog.service';
import { Observable } from 'rxjs';
import { MatStepperTab } from '../../shared/enums.service';
import { NotificationService } from '../../services/NotificationService';

@Component({
  selector: 'app-translate',
  templateUrl: './translate.component.html',
  styleUrls: ['./translate.component.css']
})

export class TranslateComponent implements OnInit {
  displayedColumns =
    ['select', 'actions', 'wfL_ID', 'smT_STA_NR', 'pkG_NR_TE', 'rcV_CPY_TE', 'rcV_ADR_TE', 'shP_ADR_TR_TE', 'coN_NR', 'acY_TE',
      'dsT_CTY_TE', 'dsT_PSL_TE', 'csG_CTC_TE', 'pH_NR', 'fsT_INV_LN_DES_TE', 'shP_CPY_NA', 'shP_ADR_TE', 'shP_CTC_TE', 'shP_PH_TE',
      'orG_CTY_TE', 'orG_PSL_CD', 'imP_SLC_TE', 'coD_TE', 'poD_RTN_SVC'
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
  public dataForTranslate: any[] = [];
  filterText: string = '';

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    private router: Router, public dialog: MatDialog,
    public dataService: DataService,
    private snackBar: MatSnackBar,
    private dialogService: DialogService,
    private notificationService: NotificationService) {
  }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnInit() {
    this.WorkflowID = this.activatedRoute.snapshot.params.WorkflowID;
    this.eventsSubscription = this.events.subscribe((event: any) => {
      let selectedTabIndex = event.selectedIndex;
      if (this.WorkflowID && selectedTabIndex == MatStepperTab.TranslatedTab) {
        this.getTranslateData(this.WorkflowID)
      }
    });
  }

  ngOnDestroy() {
    this.eventsSubscription.unsubscribe()
  }

  getTranslateData(WorkflowID: any) {
    this.shippingService.getTranslateData(WorkflowID).subscribe((response: any) => {
      if (response) {
        this.dataSource.data = response;
      } else {
        this.dataSource.data = [];
      }
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.selection.clear();
      this.filterText = '';
      this.applyFilter('');
    }, error => (this.errorMessage = <any>error));
  }

  applyFilter(filterValue: string) {
    this.filterText = filterValue;
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  isAllSelected() {
    const MainData: any[] = this.dataSource._pageData(this.dataSource.data);
    const ValidData: any[] = MainData.filter(data => (data.smT_STA_NR !== 2 && data.smT_STA_NR !== 3));
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
    this.mainData = this.dataSource._pageData(this.dataSource.data);
    this.checkedData = this.mainData.filter(data => (data.smT_STA_NR !== 2 && data.smT_STA_NR !== 3));
    this.isAllSelected() ?
      this.selection.clear() :
      this.selection.clear(),
      this.checkedData.forEach(row => this.selection.select(row));
  }

  handlePageChange(event: Event) {
    // this.selection.clear();
  }
  
  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  rowChecked(event: Event, row: any) {
    event.stopPropagation();
    if (!this.selection.isSelected(row)) {
      if (this.selection.selected.length >= 100) {
        this.dialogService.openAlertDialog('Maximum allowed Shipments for Translation: 100 and You have selected: ' + this.selection.selected.length);
        this.selection.toggle(row);
      }
    }
  }

  /** Method to Translate the Data*/
  public sendForTranslate() {
    const checkedCount = this.selection.selected.length;
    if (checkedCount <= 0) {
      this.dialogService.openAlertDialog('Please select minimum one row to Translate.');
    } else if (checkedCount > 100) {
      this.dialogService.openAlertDialog('Maximum allowed Shipments for Translation: 100 and You have selected: ' + this.selection.selected.length);
    } else {
      const data = this.selection.selected;
      this.dataTranslate(data);
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
        poD_RTN_SVC: shipmentDetailToUpdate.poD_RTN_SVC
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
          SHP_ADR_TR_TE: updatedDetails.shP_ADR_TR_TE,
          COD_TE: updatedDetails.coD_TE,
          WFL_ID: shipmentDetails.wfL_ID,
          ID: shipmentDetails.id,
          POD_RTN_SVC: updatedDetails.poD_RTN_SVC
        }

        this.shippingService.UpdateShippingAddress(details).subscribe((response: any) => {
          console.log(response)
          shipmentDetailToUpdate.shP_ADR_TR_TE = response.shipmentDataRequest.shP_ADR_TR_TE;
          shipmentDetailToUpdate.coD_TE = response.shipmentDataRequest.coD_TE;
          shipmentDetailToUpdate.smT_STA_NR = response.shipmentDataRequest.smT_STA_NR;
          shipmentDetailToUpdate.poD_RTN_SVC = response.shipmentDataRequest.poD_RTN_SVC;
          this.notificationService.openSuccessMessageNotification("Data Updated Successfully.");
        },
          error => this.notificationService.openErrorMessageNotification("Error while updating data."))
      }
    });
  }

  rowTranslate(i, shipmentWorkFlowRequest) {
    this.dataTranslate([shipmentWorkFlowRequest]);
  };

  dataTranslate(data: any) {
    this.dataForTranslate = [];
    const dataTranslate = data;

    for (let mainData of dataTranslate) {
      if (mainData.dsT_CTY_TE === null) { mainData.dsT_CTY_TE = '' };
      if (mainData.rcV_ADR_TE === null) { mainData.rcV_ADR_TE = '' };
      this.dataForTranslate.push(mainData);
    }

    this.shippingService.sendDataForTranslate(this.dataForTranslate).subscribe(
      (response: any) => {
        if (response.geocode.length > 0) {
          var EmptyCount: number = 0;
          var SuccessCount: number = 0;

          for (let geocode of response.geocode) {
            if (geocode.translated_adddress === ' ') {
              EmptyCount = EmptyCount + 1;
            } else {
              SuccessCount = SuccessCount + 1;
            }
          }

          const data = {
            emptyCount: EmptyCount,
            successCount: SuccessCount,
            screenFrom: 'Translate'
          }
          this.dialogService.openSummaryDialog(data);
        }

        //this.notificationService.openSuccessMessageNotification("Shipment Address(es) Translated Successfully.");
        this.getTranslateData(this.WorkflowID);
        this.selection.clear();
      }
      ,
      error => this.notificationService.openErrorMessageNotification("Error while Translating data.")
    );
  }
}
