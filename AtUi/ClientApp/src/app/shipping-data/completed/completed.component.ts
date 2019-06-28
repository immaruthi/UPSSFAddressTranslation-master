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
import { ExcelService } from '../../services/ExcelExport';
import { MatStepperTab } from '../../shared/enums.service';

@Component({
  selector: 'app-completed',
  templateUrl: './completed.component.html',
  styleUrls: ['./completed.component.css']
})
export class CompletedComponent implements OnInit {
  displayedColumns =
    ['smT_STA_NR', 'pkG_NR_TE', 'rcV_CPY_TE', 'rcV_ADR_TE', 'shP_ADR_TR_TE', 'dsT_CTY_TE', 'dsT_PSL_TE',
      'fsT_INV_LN_DES_TE', 'shP_CPY_NA', 'shP_ADR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD',
          'imP_SLC_TE', 'coD_TE', 'pyM_MTD', 'exP_TYP', 'spC_SLIC_NR'
    ];
  private eventsSubscription: any;
  @Input() events: Observable<void>;

  public ResponseData: any[] = [];
  public WorkflowID: any;
  public shipmentStatusList = Constants.ShipmentStatusList;
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);
  public mainData: any[] = [];
  public checkedData: any[] = [];
  public tableData: any[] = [];
  public excelMainData: any[] = [];

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    private router: Router, public dialog: MatDialog, public dataService: DataService,
    private snackBar: MatSnackBar, private dialogService: DialogService,
    private excelService: ExcelService) {
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
    this.eventsSubscription = this.events.subscribe((event:any) => {
      let selectedTabIndex = event.selectedIndex;
      if (this.WorkflowID && selectedTabIndex == MatStepperTab.CompletedTab) {
        this.getCompletedShipments(this.WorkflowID);
      }
      
    });
  }

  ngOnDestroy() {
    this.eventsSubscription.unsubscribe()
  }

  getCompletedShipments(WorkflowID: any) {
    this.ResponseData = [];
    this.shippingService.getCompletedShipments(WorkflowID).subscribe((response: any) => {
      if (response.success === true) {
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

  exportToExcel() {
    this.tableData = [];
    this.excelMainData = [];
    this.tableData = this.dataSource.data;
    if (this.tableData.length > 0) {
      for (let data of this.tableData) {
        this.excelMainData.push(
          {
            'SHP Status': this.shipmentStatusList[data.smT_STA_NR].value,
            'Package Number': data.pkG_NR_TE,
            'Receiving Company': data.rcV_CPY_TE,
            'Receiving Address': data.rcV_ADR_TE,
            'Translated Address': data.shP_ADR_TR_TE,
            'Receiving City': data.dsT_CTY_TE,
            'Receiving Postal Code': data.dsT_PSL_TE,
            'Specification': data.fsT_INV_LN_DES_TE,
            'SHP Company Name': data.shP_CPY_NA,
            'SHP Address': data.shP_ADR_TE,
            'SHP Contact': data.shP_CTC_TE,
            'SHP Phone': data.shP_PH_TE,
            'Origin City': data.orG_CTY_TE,
            'Origin Postal code': data.orG_PSL_CD,
            'IMP SLC': data.imP_SLC_TE,
            'COD': data.coD_TE,
            'Payment Method': data.pyM_MTD,
            'Express Type': data.exP_TYP,
            'Slic': data.spC_SLIC_NR
          })
      }
    } else {
      this.dialogService.openAlertDialog('No data for export.');
    }
    this.excelService.exportAsExcelFile(this.excelMainData, 'Completed');
  }

}
