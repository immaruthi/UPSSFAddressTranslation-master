import { Component, OnInit } from '@angular/core';
import { ShipmentDetails } from '../../models/shipmentDetails';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-sent-to-sf',
  templateUrl: './sent-to-sf.component.html',
  styleUrls: ['./sent-to-sf.component.css']
})
export class SentToSfComponent implements OnInit {
  displayedColumns =
    ['WFL_ID', 'SMT_NR_TE', 'SHP_ADR_TE', 'RCV_ADR_TE', 'RCV_ADR_CN', 'DST_CTY_TE',
      'DST_PSL_TE', 'Status'
    ];
  dataSource = new MatTableDataSource<ShipmentDetails>(ELEMENT_DATA);
  selection = new SelectionModel<ShipmentDetails>(true, []);
  constructor() { }

  ngOnInit() {
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }
  checkboxLabel(row?: ShipmentDetails): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    //return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }
}
const ELEMENT_DATA: ShipmentDetails[] = [
  {
    WFL_ID: 1, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'SUITE 601_602,SHANGHAI TIMES SQUARE NO:93 HUAI HAI MIDDLE ROAD LUWAN D.', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '201137', Status: 'Uploaded'
  },
  {
    WFL_ID: 2, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '77 ADELAIDE ST W RM/STE 1 ON', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200177', Status: 'Uploaded'
  },
  {
    WFL_ID: 3, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200337', Status: 'Uploaded'
  },
  {
    WFL_ID: 4, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '1384 BROADWAY 25TH FLOOR NY', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200157', Status: 'Uploaded'
  },
  {
    WFL_ID: 5, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '77 ADELAIDE ST W RM/STE 1 ON', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200134', Status: 'Uploaded'
  },
  {
    WFL_ID: 5, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '77 ADELAIDE ST W RM/STE 1 ON', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200134', Status: 'Uploaded'
  },
  {
    WFL_ID: 5, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '77 ADELAIDE ST W RM/STE 1 ON', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200134', Status: 'Uploaded'
  },
  {
    WFL_ID: 5, SMT_NR_TE: 'AF3977VBWDD', SHP_ADR_TE: '77 ADELAIDE ST W RM/STE 1 ON', RCV_CPY_TE: 'BASF CHEMICALS CO., LTD', RCV_ADR_TE: 'NO.7 BLDG, NO.96 ZHAOJIABANG RD. ', RCV_ADR_CN: '', DST_CTY_TE: 'SHANGHAI', DST_PSL_TE: '200134', Status: 'Uploaded'
  },

];
