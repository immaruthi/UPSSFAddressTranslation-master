import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { LogFilesService } from '../services/LogFilesService';
import { ExcelService } from '../services/ExcelExport';
import { DialogService } from '../services/dialog.service';
import { DatePipe } from '@angular/common'

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
  tableData;
  excelMainData;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  LoadLogFilesList() {
    this.logService.GetLogFilesList().subscribe((result: any) => { this.logFilesList = result; });
  }

  onclicklogfile(filename: any) {
    this.logGrid = [];
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

  LogsexportToExcel() {
    this.tableData = [];
    this.excelMainData = [];
    this.tableData = this.dataSource.data;
    if (this.tableData.length > 0) {
      for (let data of this.tableData) {
        this.excelMainData.push(
          {
            'Date': this.datepipe.transform(data.dateTime, 'MMMM dd, yyyy hh:mm a'),
            'User Id': data.userID,
            'Application Name': data.apiType,
            'Request': data.logRequest,
            'Response': data.logResponse,
            'Exception': data.logException,
          })
      }
      this.excelService.exportAsExcelFile(this.excelMainData, 'Log_Report@');
    } else {
      this.dialogService.openAlertDialog('No data for export.');
    }
  }

  constructor(private logService: LogFilesService, private excelService: ExcelService,
    private dialogService: DialogService, private datepipe: DatePipe) { }  

  ngOnInit() {
    this.LoadLogFilesList();
    this.dataSource.data = this.logGrid;
    this.dataSource.paginator = this.paginator;
    this.search = true;
  }

  onRowClick(i: number, row: any) {
    var matRow = <HTMLElement>document.getElementById('matTable');
    for (let j = 1; j < matRow.childElementCount; j++) {
      if (j === (i + 1)) {
        //matRow.children[j].setAttribute('style', 'height: 100% !important');
        const currentStyle = matRow.children[j].getAttribute('style');

        if (currentStyle === 'height: 100% !important') {
          matRow.children[j].setAttribute('style', 'height: 56px !important');
        } else {
          matRow.children[j].setAttribute('style', 'height: 100% !important');
        }
      } else {
        matRow.children[j].setAttribute('style', 'height: 56px !important');
      }
    }
  }
}


