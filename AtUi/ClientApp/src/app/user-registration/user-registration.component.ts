import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UserService } from '../services/UserService';
import { UserReg } from '../models/UserReg';
import { DialogService } from '../services/dialog.service';
import { error } from 'util';
import { MatTableDataSource, MatPaginator, MatDialogConfig, MatDialog } from '@angular/material';
import { Constants } from '../shared/Constants';
import { AddUserComponent } from './add-user/add-user.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent implements OnInit {

  displayedColumns =
    ['edit', 'firstName', 'lastName', 'email', 'userId', 'cities', 'role'];

  hide = true;
  userreg: UserReg;
  selected;
  cities;
  usersList: UserReg[] = [];
  dataSource = new MatTableDataSource<UserReg>();
  Roles = Constants.userRoles;
  filterText: string = '';
  selection = new SelectionModel<any>(true, []);

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(public userservice: UserService, private dialogService: DialogService, private dialog: MatDialog) { }

  ngOnInit() {
    this.GetAllCities();
    this.GetAllUsers();
  }


  GetAllCities() {
    this.userservice.GetAllCities().subscribe((data) => {
      this.cities = data;
    });
  }
  GetAllUsers() {
    this.userservice.GetAllUsers().subscribe((data) => {
      this.usersList = data;
      this.dataSource.data = this.usersList;
      this.dataSource.paginator = this.paginator;
    })
  }

  openDialog() {
    this.userservice.intiliazeFormGroup();
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = "50%";
    dialogConfig.autoFocus = true;
    this.dialog.open(AddUserComponent, dialogConfig).afterClosed().subscribe(result => {
      this.GetAllUsers();
    });
  }

  onClickEdit(user) {
    this.userservice.onEdit(user);
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = "50%";
    this.dialog.open(EditUserComponent, dialogConfig).afterClosed().subscribe(result => {
      this.GetAllUsers();
    });
  }

  applyFilter(filterValue: string) {
    this.filterText = filterValue;
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
    this.selection.clear();
  }
}
