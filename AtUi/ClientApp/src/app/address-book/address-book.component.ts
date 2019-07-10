import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatDialog, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { AddressBookService } from '../services/AddressBookService';
import { ShipperListService } from '../services/ShipperListService';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../services/data.service';
import { Observable } from 'rxjs';
import { NotificationService } from '../services/NotificationService';
import { AddressBookEditModelComponent } from './address-book-edit-model/address-book-edit-model.component';

@Component({
  selector: 'app-address-book',
  templateUrl: './address-book.component.html',
  styleUrls: ['./address-book.component.css']
})
export class AddressBookComponent implements OnInit {

  //displayedColumns =
  //  ['spC_PSL_CD_TE', 'spC_CTY_TE', 'spC_CTR_TE', 'spC_CPY_TE', 'spC_NA', 'spC_SND_PTY_CTC_TE', 'spC_ADR_TE', 'spC_CTC_PH', 'spC_SLIC_NR',];

  displayedColumns =
    ['actions','ShipmentId', 'BatchId', 'Organization', 'StatusCode', 'ConsigneeAddressId', 'ConsigneeAddress',
      'ConsigneeTranslatedAddress', 'Address_One', 'Address_Two', 'Address_Three', 'Address_Four', 'Road',
      'City', 'Region', 'Country', 'AddressTypeFlag', 'Longitude', 'Latitude', 'GeoCode', 'GeoCodeError',
      'BuldingNumber', 'BuildingName', 'Unit', 'Area', 'Bat_Id', 'PostalCode', 'Confidence', 'SemanticCheck',
      'Accuracy', 'VerifyMatch', 'CreatedDate', 'ModifiedDate'];

  private eventsSubscription: any;
  @Input() events: Observable<void>;

  public ResponseData: any[] = [];
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;
  selection = new SelectionModel<any>(true, []);
  filterText: string = '';

  constructor(private activatedRoute: ActivatedRoute,
    public dialog: MatDialog, public dataService: DataService, public shipperListService: ShipperListService,
    private addressBookService: AddressBookService, private notificationService: NotificationService) {
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

  startEdit(i: number, addressBookData: any) {
    let addressBookDetails = addressBookData;
    const dialogRef = this.dialog.open(AddressBookEditModelComponent, {
      data: {
        Id: addressBookData.id,
        ConsigneeAddress: '5 /F. NO.3 BLDG. 318  XUEYUAN  ROAD YINZHOU DISTRICT HAISU DISTRICT, ZHEJ', // addressBookData.ConsigneeAddress,
        ConsigneeTranslatedAddress: '5号 / f。宁波市浙江省菏州市海曙区学院路318栋3楼'// addressBookData.ConsigneeTranslatedAddress
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        let updatedDetails = this.dataService.getDialogData();

        if (updatedDetails.ConsigneeTranslatedAddress == addressBookData.ConsigneeTranslatedAddress) {

          this.notificationService.openSuccessMessageNotification("No changes found to update");
          return;
        }

        const details = {
          ID: addressBookDetails.id,
          ConsigneeTranslatedAddress: updatedDetails.ConsigneeTranslatedAddress
        }

        this.addressBookService.updateAddressBook(details).subscribe((response: any) => {
          console.log(response)

          addressBookDetails.shP_ADR_TR_TE = response.shipmentDataRequest.shP_ADR_TR_TE;;
          addressBookDetails.coD_TE = response.shipmentDataRequest.coD_TE;
          addressBookDetails.smT_STA_NR = response.shipmentDataRequest.smT_STA_NR;
          addressBookData.poD_RTN_SVC = response.shipmentDataRequest.poD_RTN_SVC;
          this.notificationService.openSuccessMessageNotification("Data Updated Successfully.");
        },
          error => this.notificationService.openErrorMessageNotification("Error while updating data."))
      }
    });
  }

}
