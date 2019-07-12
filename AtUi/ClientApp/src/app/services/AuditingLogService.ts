import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';

@Injectable()
export class AuditingLogService {
  constructor(private httpClient: HttpClient, private router: Router,
    private httpService: HttpService) { }

  public getAddressAuditLogData(): Observable<any> {
    return this.httpService.makeGetRequest('api/Shipment/GetAddressAuditLogData');
  }
}
