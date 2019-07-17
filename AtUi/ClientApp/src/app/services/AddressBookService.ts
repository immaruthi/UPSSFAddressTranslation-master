import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpService } from '../shared/http.service';
import { AddressBookData } from '../models/AddressBook';

@Injectable()
export class AddressBookService {
  constructor(private httpClient: HttpClient, private router: Router,
    private httpService: HttpService) { }

  public getAddressBookData(): Observable<AddressBookData[]> {
    return this.httpService.makeGetRequest('api/AddressBook/getall');
  }

  public updateAddressBook(data: any): Observable<any> {
    return this.httpService.makePostRequest('api/AddressBook/UpdateAddressBookById/', data);
  }
}
