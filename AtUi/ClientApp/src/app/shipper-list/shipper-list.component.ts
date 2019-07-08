import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatDialog, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { ShippingService } from '../services/shipping.service';
import { ShipperListService } from '../services/ShipperListService';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-shipper-list',
  templateUrl: './shipper-list.component.html',
  styleUrls: ['./shipper-list.component.css']
})
export class ShipperListComponent implements OnInit {

  displayedColumns =
    ['spC_PSL_CD_TE', 'spC_CTY_TE', 'spC_CTR_TE', 'spC_CPY_TE', 'spC_NA', 'spC_SND_PTY_CTC_TE', 'spC_ADR_TE', 'spC_CTC_PH','spC_SLIC_NR',];

  private eventsSubscription: any;
  @Input() events: Observable<void>;

  public ResponseData: any[] = [];
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);
  filterText: string = '';

  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
    public dialog: MatDialog, public dataService: DataService,
    private shipperListService: ShipperListService) {
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
    this.getShipperListData();
  }

  
  getShipperListData() {
    this.ResponseData = [];
    this.shipperListService.getShipperListData().subscribe((response: any) => {
      if (response) {
        if (response.success === true) {
          this.ResponseData = response.shipperCompanies;
        } else {
          this.ResponseData = [];
        }
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

  addNewRow() {
    const data = this.dataSource.data;
    //data.push({});
    this.dataSource.data = data;
  }

  addNew() {

  }
}
