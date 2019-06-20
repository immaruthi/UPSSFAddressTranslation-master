import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';

@Injectable()
export class ShippingService {
  constructor(private httpClient: HttpClient, private router: Router, private httpService: HttpService) { }

  getTranslate(WorkflowID: any) {
    //const params = new HttpParams()
    //  .set('userId', userId)
    //  .set('password', password);
    //return this.httpClient.get(`api/Login/ValidateUser`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUser`, { params })
    this.httpService.makeGetRequest('api/Shipment', [{ wid: 1 }]);
  }

  public getTranslateData(WorkflowID: any): Observable<any[]> {
    return this.httpService.makeGetRequest('api/Shipment?wid=' + WorkflowID);
  }
}
