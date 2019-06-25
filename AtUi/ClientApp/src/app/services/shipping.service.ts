import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';
import { ShipmentDetails } from '../models/shipmentDetails';
import { shipmentStatus } from '../shared/enums.service';

@Injectable()
export class ShippingService {
  constructor(private httpClient: HttpClient, private router: Router,
    private httpService: HttpService) { }

  public getUploadedData(WorkflowID: any): Observable<any> {
    return this.httpService.makeGetRequest('api/Shipment/GetShipmentData?wid=' + WorkflowID);
  }
  
  public getTranslateData(WorkflowID: any): Observable<any> {
    return this.httpService.makeGetRequest('api/Shipment/GetShipmentData?wid=' + WorkflowID);
  }

  public getDataForSendToSF(WorkflowID: any): Observable<any> {
    return this.httpService.makeGetRequest('api/Shipment/GetShipmentListData?wid=' + WorkflowID);
  }

  public sendDataForTranslate(data: any): Observable<any> {
    return this.httpService.makePostRequest('api/Shipment/GetTranslationAddress', data);  // Add URL here for send for translate
  }

  public sendDataToSF(data: any[]): Observable<any[]> {
    return this.httpService.makePostRequestXML('api/Shipment/CreateOrderShipment', data);
  }

  public UpdateShippingAddress(data: ShipmentDetails): Observable<ShipmentDetails> {

    return this.httpService.makePostRequest('api/Shipment/UpdateShipmentAddressById', data);
  }
}
