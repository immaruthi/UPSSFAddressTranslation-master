import { Component, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { UserService } from '../services/UserService';


import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';  
import * as XLSX from 'xlsx';
/**
 * @title Table with pagination
 */
@Component({
  selector: 'workflow',
  styleUrls: ['workflow.component.scss'],
  templateUrl: 'workflow.component.html',
})
export class WorkflowComponent {
  arrayBuffer: any;
  file: File;
  fileToUpload: File = null;
  //displayedColumns = ['position', 'name', 'weight', 'symbol'];
  displayedColumns = ['id', 'usR_FST_NA', 'flE_NA', 'wfL_STA_TE', 'crD_DT'];
  dataSource = new MatTableDataSource<Element>();
  constructor(private userService: UserService) {

  }

  @ViewChild(MatPaginator) paginator: MatPaginator;

  //set paginator(value: MatPaginator) {
  //  this.dataSource.paginator = value;
  //}


  /**
  * Set the paginator after the view init since this component will
  * be able to query its view for the initialized paginator.
  */
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {;
    var user = localStorage.getItem("Emp_Id");
    this.userService.getAllWorkflows(user)
      .subscribe((data: any) => {

        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        //console.log(this.arrayBuffer);
      });
    //this.dataSource = getAllWorkflows();
} 

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  onFileChange(evt: any) {
    debugger;
    /* wire up file reader */
    const target: DataTransfer = <DataTransfer>(evt.target);
    if (target.files.length !== 1) throw new Error('Cannot use multiple files');
    const reader: FileReader = new FileReader();
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });

      /* grab first sheet */
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];

      /* save data */
      //this.data = <AOA>(XLSX.utils.sheet_to_json(ws, { header: 1 }));
    };
    reader.readAsText(target.files[0]);
  }

  handleFileInput(files: FileList) {
    var user = localStorage.getItem("Emp_Id");
    this.fileToUpload = files.item(0);
    this.userService.postFile(this.fileToUpload,user)
      .subscribe((data: any) => {

        this.dataSource.data = data;
           this.dataSource.paginator = this.paginator;
        //console.log(this.arrayBuffer);
      });
  }

 
}



export interface Element {
  id: number;
  usR_FST_NA: string;
  
  //udT_DT: string;
  flE_NA: string;
  wfL_STA_TE: string;
  crD_DT: string;
 
}


