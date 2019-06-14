import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';
import { Customer } from '../models/Customer';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class CustomerService {
  constructor(private http: HttpClient) { }

  GetAllCustomer() {
    return this.http.get<Customer[]>(`api/Customer/getallcustomer`)
  }
  AddCustomer(customer: Customer[]): Observable<Customer[]> {

    let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.post<Customer[]>('api/Customer/AddCustomer', JSON.stringify(customer), options);

  }
  UpdateCustomer(customer) {

    let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.put('api/Customer/UpdateCustomer', customer);

  }
  DeleteCustomer(cust_id: string) {

    const params = new HttpParams()
      .set('cust_id', cust_id);
    return this.http.get('api/Customer/DeleteCustomer', { params });
  }


}
