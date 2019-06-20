import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';

@Injectable()
export class ShippingService {
  constructor(private httpClient: HttpClient, private router: Router) { }

  getTranslateData(WorkflowID: any) {
    //const params = new HttpParams()
    //  .set('userId', userId)
    //  .set('password', password);
    //return this.httpClient.get(`api/Login/ValidateUser`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUser`, { params })
  }

  
}
