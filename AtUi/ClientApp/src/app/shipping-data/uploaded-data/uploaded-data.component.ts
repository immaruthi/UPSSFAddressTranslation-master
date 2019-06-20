
import { Component, OnInit } from '@angular/core';
import { ShipmentDetails } from '../../models/shipmentdetails';

@Component({
  selector: 'app-uploaded-data',
  templateUrl: './uploaded-data.component.html',
  styleUrls: ['./uploaded-data.component.css']
})
export class UploadedDataComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }
  displayedColumns =
    ['WFL_ID', 'SMT_NR_TE', 'SHP_ADR_TE', 'RCV_ADR_TE', 'RCV_ADR_CN', 'DST_CTY_TE',
      'DST_PSL_TE','Status'
    ];
  dataSource = ELEMENT_DATA;
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

