import { Component } from '@angular/core';

/**
 * @title Basic table
 */
@Component({
  selector: 'shipment-data',
  styleUrls: ['shipment.component.scss'],
  templateUrl: 'shipment.component.html',
})
export class ShipmentComponent {
 // displayedColumns = ['position', 'name', 'weight', 'symbol'];
  displayedColumns = ['WorkflowID', 'Owner', 'FileName', 'Status', 'CreatedAt'];
  dataSource = ELEMENT_DATA;
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
