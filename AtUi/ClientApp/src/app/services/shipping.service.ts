import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';

@Injectable()
export class ShippingService {
  constructor(private httpClient: HttpClient, private router: Router, private httpService: HttpService) { }

  //public getTranslate(WorkflowID: any) {
  //  this.httpService.makeGetRequest('api/Shipment', [{ wid: WorkflowID }]);
  //}

  public getTranslateData(WorkflowID: any): Observable<any[]> {
    return this.httpService.makeGetRequest('api/Shipment?wid=' + WorkflowID);
  }

  public sendDataForTranslate(data: any[]): Observable<any[]> {
    return this.httpService.makePostRequest('', data);  // Add URL here for send for translate
  }
}
