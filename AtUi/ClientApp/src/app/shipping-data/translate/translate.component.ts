import { Component, ViewChild, OnInit } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-translate',
  templateUrl: './translate.component.html',
  styleUrls: ['./translate.component.css']
})
export class TranslateComponent implements OnInit {
  displayedColumns =
    ['select', 'WFL_ID', 'SMT_NR_TE', 'SHP_DT',
      'SHP_CPY_NA', 'SHP_ADR_TE', 'SHP_ADR_TR_TE', 'ORG_CTY_TE', 'ORG_PSL_CD', 'RCV_CPY_TE', 'RCV_ADR_TE', 'DST_CTY_TE',
      'DST_PSL_TE'
    ];
  //dataSource = new MatTableDataSource<Element>();
  selection = new SelectionModel<any>(true, []);
  constructor() {

  }

  @ViewChild(MatPaginator) paginator: MatPaginator;



  /**
  * Set the paginator after the view init since this component will
  * be able to query its view for the initialized paginator.
  */
  ngAfterViewInit() {
    //this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.length;
    return numSelected === numRows;
  }

  //// Editable function
  //updateField(index, field) {
  //  const control = this.getControl(index, field);
  //  if (control.valid) {
  //    this.core.update(index, field, control.value);
  //  }

  //}

  //getControl(index, fieldName) {
  //  const a = this.controls.at(index).get(fieldName) as FormControl;
  //  return this.controls.at(index).get(fieldName) as FormControl;
  //}
  //// Editable Ends

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  dataSource = [
    {
      WFL_ID: 1, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', SHP_ADR_TR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
    },
    {
      WFL_ID: 2, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', SHP_ADR_TR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
    },
    {
      WFL_ID: 3, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', SHP_ADR_TR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
    },
    {
      WFL_ID: 4, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', SHP_ADR_TR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
    },
    {
      WFL_ID: 5, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', SHP_ADR_TR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
    },
    {
      WFL_ID: 6, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', SHP_ADR_TR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
    },
  ];
  updateField(index, field) {
    debugger;
    const control = this.getControl(index, field);
    //if (control.valid) {
    //  //this.core.update(index, field, control.value);
    //}

  }

  getControl(index, fieldName) {
    debugger;
    //const a = this.controls.at(index).get(fieldName) as FormControl;
    //return this.controls.at(index).get(fieldName) as FormControl;
  }
}
