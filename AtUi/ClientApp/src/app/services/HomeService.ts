import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HomeService {

  constructor(private _http: HttpClient) {  }

  getEmployeeCount() { return this._http.get('api/Home/GetallEmployeeCount') }

  getEmpProjCount() { return this._http.get('api/Home/GetallEmpProjCount') }

  getEmpBenchCount() { return this._http.get('api/Home/GetallEmpBenchCount') }

  getProjCount() { return this._http.get('api/Home/GetallProjCount') }

  getCustCount() { return this._http.get('api/Home/GetallCustomerCount') }

}
