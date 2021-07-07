import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatDialog, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { ShippingService } from '../services/shipping.service';
import { ShipperListService } from '../services/ShipperListService';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { AuditingLogService } from '../services/AuditingLogService';
import { DialogService } from '../services/dialog.service';
import { DatePipe } from '@angular/common'

@Component({
  selector: 'app-auditing-log',
  templateUrl: './auditing-log.component.html',
  styleUrls: ['./auditing-log.component.css']
})
export class AuditingLogComponent implements OnInit {



  displayedColumns =
    ['wfL_ID','smT_ID', 'csG_ADR', 'bfR_ADR', 'afR_ADR', 'upD_BY_TE', 'upD_DT', 'upD_FRM'];

  public ResponseData: any[] = [];
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);
  filterText: string = '';

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    public dialog: MatDialog, public dataService: DataService,
    private auditingLogService: AuditingLogService, private dialogService: DialogService, private datepipe: DatePipe) {
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
    this.getAuditingLogData();
  }


  getAuditingLogData() {
    this.ResponseData = [];
    this.auditingLogService.getAddressAuditLogData().subscribe((response: any) => {
      if (response) {
        this.ResponseData = response;
        this.dataSource.data = this.ResponseData;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.filterText = '';
        this.applyFilter('');
      }
    }, error => (this.errorMessage = <any>error));
  }

  applyFilter(filterValue: string) {
    this.filterText = filterValue;
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  onDateFilter() {
    const FROM = (<HTMLInputElement>document.getElementById('fromDate')).value;
    const TO = (<HTMLInputElement>document.getElementById('toDate')).value;

    if (FROM && TO) {
      const fromdate = this.datepipe.transform(FROM, 'yyyy-MM-dd');
      const todate = this.datepipe.transform(TO, 'yyyy-MM-dd');
      if (fromdate > todate) {
        this.dialogService.openAlertDialog('Invalid dates for search.');
      } else {
        this.applyFilter('');
        this.dataSource.data = this.ResponseData.filter(data => {
          var rowDate = this.datepipe.transform(data.upD_DT, 'yyyy-MM-dd');
          return (rowDate >= fromdate && rowDate <= todate);
        });
      }
    } else {
      this.dialogService.openAlertDialog('Please select From and To dates to search.');
    }
  }

  onResetDateSearch() {
    this.applyFilter('');
    (<HTMLInputElement>document.getElementById('fromDate')).value = ' ';
    (<HTMLInputElement>document.getElementById('toDate')).value = ' ';
    this.dataSource.data = this.ResponseData;
  }

}
