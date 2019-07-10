import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';

@Injectable()
export class AddressBookService {
  constructor(private httpClient: HttpClient, private router: Router,
    private httpService: HttpService) { }

  public getAddressBookData(): Observable<any> {
    return this.httpService.makeGetRequest(' ');
  }

  public updateAddressBook(data: any): Observable<any> {
    return this.httpService.makePostRequest(' ', data);
  }
}
