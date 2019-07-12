import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { LogFilesService } from '../services/LogFilesService';

@Component({
  selector: 'app-log-grid',
  templateUrl: './log-grid.component.html',
  styleUrls: ['./log-grid.component.css']
})

export class LogGridComponent implements OnInit {
  logFilesList = [];
  logGrid = [];
  dataSource = new MatTableDataSource<any>();
  displayedColumns: string[] = ['logDate', 'userId', 'apiType', 'logRequest', 'logResponse', 'logException'];
  search = true;
  filterText: string = '';

  @ViewChild(MatPaginator) paginator: MatPaginator;

  LoadLogFilesList() {
    this.logService.GetLogFilesList().subscribe((result: any) => { this.logFilesList = result; });
  }

  onclicklogfile(filename: any) {
    this.logService.GetLogGrid(filename).subscribe((result: any) => {
      //this.logGrid = result;
      for (let data of result) {
        this.logGrid.push(
          {
            dateTime: data.dateTime,
            userID: data.userID,
            apiType: data.apiType,
            logRequest: data.logInformation.logRequest,
            logResponse: data.logInformation.logResponse,
            logException: data.logInformation.logException
          })
      }
      this.dataSource.data = this.logGrid;
      this.search = false;
    });    
  }

  applyFilter(filterValue: string) {
    this.filterText = filterValue;
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  constructor(private logService: LogFilesService) { }  

  ngOnInit() {
    this.LoadLogFilesList();
    this.dataSource.data = this.logGrid;
    this.dataSource.paginator = this.paginator;
    this.search = true;
  } 
}


