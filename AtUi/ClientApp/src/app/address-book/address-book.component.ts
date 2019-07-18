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
import { AddressBookData } from '../models/AddressBook';

@Component({
  selector: 'app-address-book',
  templateUrl: './address-book.component.html',
  styleUrls: ['./address-book.component.css']
})
export class AddressBookComponent implements OnInit {

  //displayedColumns =
  //  ['spC_PSL_CD_TE', 'spC_CTY_TE', 'spC_CTR_TE', 'spC_CPY_TE', 'spC_NA', 'spC_SND_PTY_CTC_TE', 'spC_ADR_TE', 'spC_CTC_PH', 'spC_SLIC_NR',];

  displayedColumns =
    ['actions', 'consigneeAddressId', 'consigneeAddress', 'consigneeTranslatedAddress',
      'confidence', 'accuracy', 'createdDate', 'modifiedDate', 'organization', 'batchId', 'statusCode', 'address_One', 'address_Two',
      'address_Three', 'address_Four', 'road', 'city', 'region', 'country', 'addressTypeFlag', 'longitude',
      'latitude', 'geoCode', 'geoCodeError', 'buldingNumber', 'buildingName', 'unit', 'area', 'bat_Id',
      'postalCode', 'semanticCheck', 'verifyMatch'];

  public ResponseData: AddressBookData[] = [];
  dataSource = new MatTableDataSource<AddressBookData>();
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
    this.getAddressBookData();
  }


  getAddressBookData() {
    this.ResponseData = [];
    this.addressBookService.getAddressBookData().subscribe((response: AddressBookData[]) => {
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

  startEdit(i: number, addressBookData: AddressBookData) {
    let addressBookDetails = addressBookData;
    const dialogRef = this.dialog.open(AddressBookEditModelComponent, {
      data: {
        Id: addressBookData.id,
        ConsigneeAddress: addressBookData.consigneeAddress,
        ConsigneeTranslatedAddress: addressBookData.consigneeTranslatedAddress
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 1) {
        let updatedDetails = this.dataService.getDialogData();

        if (updatedDetails.ConsigneeTranslatedAddress == addressBookData.consigneeTranslatedAddress) {

          this.notificationService.openSuccessMessageNotification("No changes found to update");
          return;
        }

        const details = {
          id: addressBookDetails.id,
          consigneeTranslatedAddress: updatedDetails.ConsigneeTranslatedAddress.trim(),
          consigneeAddress: updatedDetails.ConsigneeAddress
        }

        this.addressBookService.updateAddressBook(details).subscribe((response: any) => {
          if (response) {
            if (response.success === true) {
              addressBookDetails.consigneeTranslatedAddress = response.addressBookData.consigneeTranslatedAddress;
              addressBookDetails.modifiedDate = response.addressBookData.modifiedDate;
              this.notificationService.openSuccessMessageNotification("Data Updated Successfully.");
            } else {
              this.notificationService.openErrorMessageNotification(response.operatonExceptionMessage);
            }
          } else {
            this.notificationService.openErrorMessageNotification("Invalid exception occured, please contact administrator.");
          }

        },
          error => this.notificationService.openErrorMessageNotification(error.status + ' : ' + error.statusText))
      }
    });
  }

}
