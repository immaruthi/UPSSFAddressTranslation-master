import { Injectable } from "@angular/core";

@Injectable()
export class Constants {
  static SNAKBAR_SHOW_DURATION = 3000;
  static ShipmentStatusList = [{ key: 0, value: 'Uploaded' }, { key: 1, value: 'Curated' }, { key: 2, value: 'Translated' },
  { key: 3, value: 'Completed' }, { key: 4, value: 'NA' }
  ];
  static PODoptions = [
    { key: 0, value: '  ' },
    { key: 1, value: '签回单' }
  ];
}
