import { Injectable } from "@angular/core";

@Injectable()
export class Constants {
  static SNAKBAR_SHOW_DURATION = 3000;
  static ShipmentStatusList = [{ key: 0, value: 'Uploaded' }, { key: 1, value: 'Translated' }, { key: 2, value: 'Done' }]
}
