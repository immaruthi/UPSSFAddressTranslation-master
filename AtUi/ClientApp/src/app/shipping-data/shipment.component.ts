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

private eventsSubject: Subject<void> = new Subject<void>();
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
    this.eventsSubject.next(event);
  }
}
