import { Component } from '@angular/core';
import { ShipmentDetails } from '../models/shipmentDetails';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subject } from 'rxjs';

/**
 * @title Basic table
 */
@Component({
  selector: 'shipment-data',
  styleUrls: ['shipment.component.css'],
  templateUrl: 'shipment.component.html',
})
export class ShipmentComponent {

public eventsSubject: Subject<void> = new Subject<void>();
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  fourthFormGroup: FormGroup;

  constructor(private _formBuilder: FormBuilder) { }

  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
    this.thirdFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
    this.fourthFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }
  selectionChanged(event: any) {
    this.eventsSubject.next();
  }
}


const ELEMENT_DATA: ShipmentDetails[] = [
  {
    WFL_ID: 1, SMT_NR_TE: '', SHP_DT: '', SHP_CPY_NA: '', SHP_ADR_TE: '', ORG_CTY_TE: '',
    ORG_PSL_CD: '', RCV_CPY_TE: '', RCV_ADR_TE: '', DST_CTY_TE: '', DST_PSL_TE:''
  },
  {
    WFL_ID: 2, SMT_NR_TE: '', SHP_DT: '', SHP_CPY_NA: '', SHP_ADR_TE: '', ORG_CTY_TE: '',
    ORG_PSL_CD: '', RCV_CPY_TE: '', RCV_ADR_TE: '', DST_CTY_TE: '', DST_PSL_TE: ''
  },
  {
    WFL_ID: 3, SMT_NR_TE: '', SHP_DT: '', SHP_CPY_NA: '', SHP_ADR_TE: '', ORG_CTY_TE: '',
    ORG_PSL_CD: '', RCV_CPY_TE: '', RCV_ADR_TE: '', DST_CTY_TE: '', DST_PSL_TE: ''
  },
  {
    WFL_ID: 4, SMT_NR_TE: '', SHP_DT: '', SHP_CPY_NA: '', SHP_ADR_TE: '', ORG_CTY_TE: '',
    ORG_PSL_CD: '', RCV_CPY_TE: '', RCV_ADR_TE: '', DST_CTY_TE: '', DST_PSL_TE: ''
  },
  {
    WFL_ID: 5, SMT_NR_TE: '', SHP_DT: '', SHP_CPY_NA: '', SHP_ADR_TE: '', ORG_CTY_TE: '',
    ORG_PSL_CD: '', RCV_CPY_TE: '', RCV_ADR_TE: '', DST_CTY_TE: '', DST_PSL_TE: ''
  },
  {
    WFL_ID: 6, SMT_NR_TE: '', SHP_DT: '', SHP_CPY_NA: '', SHP_ADR_TE: '', ORG_CTY_TE: '',
    ORG_PSL_CD: '', RCV_CPY_TE: '', RCV_ADR_TE: '', DST_CTY_TE: '', DST_PSL_TE: ''
  },

  
];
