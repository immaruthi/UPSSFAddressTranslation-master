import { Component, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import * as XLSX from 'xlsx';
/**
 * @title Table with pagination
 */
@Component({
  selector: 'workflow',
  styleUrls: ['workflow.component.css'],
  templateUrl: 'workflow.component.html',
})
export class WorkflowComponent {
  arrayBuffer: any;
  file: File;
  //displayedColumns = ['position', 'name', 'weight', 'symbol'];
  displayedColumns = ['WorkflowID', 'Owner', 'FileName', 'Status', 'CreatedAt'];
  dataSource = new MatTableDataSource<Element>
    (ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;

  /**
  * Set the paginator after the view init since this component will
  * be able to query its view for the initialized paginator.
  */
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
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
//inside export class

//incomingfile(event)
//{
//  this.file = event.target.files[0];
//  this.Upload();
//}

//Upload() {
//  let fileReader = new FileReader();
//  fileReader.onload = (e) => {
//    this.arrayBuffer = fileReader.result;
//    var data = new Uint8Array(this.arrayBuffer);
//    var arr = new Array();
//    for (var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
//    var bstr = arr.join("");
//    var workbook = XLSX.read(bstr, { type: "binary" });
//    var first_sheet_name = workbook.SheetNames[0];
//    var worksheet = workbook.Sheets[first_sheet_name];
//    console.log(XLSX.utils.sheet_to_json(worksheet, { raw: true }));
//  }
//  fileReader.readAsArrayBuffer(this.file);
//}

}



export interface Element {
  WorkflowID: number;
  Owner: string;
  FileName: string;
  Status: string;
  CreatedAt: string;
}

const ELEMENT_DATA: Element[] = [
  { WorkflowID: 1, Owner: 'Aravind', FileName: 'adsd', Status: 'Translated', CreatedAt: '20-Jun-2999' },
  { WorkflowID: 2, Owner: 'James', FileName: 'sfds', Status: 'Verified', CreatedAt: '30-Jun-2029' },
  { WorkflowID: 3, Owner: 'John', FileName: 'dsd', Status: 'Done', CreatedAt: '20-Jun-2019' },
  { WorkflowID: 4, Owner: 'Wang', FileName: 'dsd', Status: 'Done', CreatedAt: '21-Jun-2029' },
  { WorkflowID: 5, Owner: 'Kelvin', FileName: 'sdsd', Status: 'Verified', CreatedAt: '30-Jun-2019' },
  { WorkflowID: 6, Owner: 'Yang', FileName: 'sdsd', Status: 'Translated', CreatedAt: '21-Jun-2019' },
];

