import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';

@Injectable()
export class ShipperListService {
  constructor(private httpClient: HttpClient, private router: Router,
    private httpService: HttpService) { }

  public getShipperListData(): Observable<any> {
    return this.httpService.makeGetRequest('api/ShipperList/GetShipmentList');
  }

  public updateShipperList(data: any): Observable<any> {
    return this.httpService.makePostRequest('api/ShipperList/UpdateShipperListById', data);
  }

  public addShipperData(data: any): Observable<any> {
    return this.httpService.makePostRequest('api/ShipperList/CreateShipmentList', data);
  }

  public deleteShipperList(data: any): Observable<any> {
    return this.httpService.makePostRequest('api/ShipperList/DeleteShipperListById', data);
  }
}
