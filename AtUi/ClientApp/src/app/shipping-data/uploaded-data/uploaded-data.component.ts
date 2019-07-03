import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { ShipmentDetails } from '../../models/shipmentdetails';
import { MatPaginator, MatTableDataSource, MatSort, MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Constants } from '../../shared/Constants';
import { Observable } from 'rxjs';
import { MatStepperTab } from '../../shared/enums.service';
import { DialogService } from '../../services/dialog.service';

@Component({
  selector: 'app-uploaded-data',
  templateUrl: './uploaded-data.component.html',
  styleUrls: ['./uploaded-data.component.css']
})
export class UploadedDataComponent implements OnInit {

  displayedColumns =
    ['actions', 'wfL_ID', 'smT_STA_NR', 'smT_NR_TE', 'rcV_CPY_TE', 'rcV_ADR_TE', 'shP_ADR_TR_TE', 'dsT_CTY_TE', 'dsT_PSL_TE',
      'csG_CTC_TE', 'pH_NR', 'fsT_INV_LN_DES_TE',
      'shP_CPY_NA', 'shP_ADR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD', 'imP_SLC_TE',
      'coD_TE'
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
    private router: Router, private snackBar: MatSnackBar, private dialogService: DialogService) {
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
    if (this.WorkflowID) {
      this.getUploadedData(this.WorkflowID);
    }
    this.eventsSubscription = this.events.subscribe((event :any) => {
      let selectedTabIndex = event.selectedIndex;
      if (this.WorkflowID && selectedTabIndex == MatStepperTab.UploadedTab) {
        this.getUploadedData(this.WorkflowID);
      }
      
    });
  }

  ngOnDestroy() {
    this.eventsSubscription.unsubscribe()
  }

  getUploadedData(WorkflowID: any) {
    this.ResponseData = [];
    this.shippingService.getUploadedData(WorkflowID).subscribe((response: any) => {
      this.ResponseData = response;
      this.dataSource.data = this.ResponseData;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
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

  delete(index: number, rowData: any) {
    this.dialogService.openAlertDialog('Work in Progress');

    // this.deleteUploadedData([rowData]);
  }

  deleteUploadedData(data: any) {
    this.shippingService.deleteUploadedData(data).subscribe((response: any) => {
      if (response) {
        this.getUploadedData(this.WorkflowID);
        this.openSuccessMessageNotification("Deleted Successfully");
      } else {
        this.openErrorMessageNotification("Error while Deleting data.");
      }
    },
      error => this.openErrorMessageNotification("Error while Deleting data."));
  }
}
