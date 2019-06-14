import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { RoleAttribute } from '../Models/RoleAttribute';

@Injectable()
export class RoleService {
  myAppUrl: string = "";
  constructor(private _http: HttpClient) {

  }
  getRole() { return this._http.get('api/Role/GetAllRoles') }

  getempdrop() { return this._http.get('api/Role/GetEmployeeDropdown') }

  saveRole(Role: RoleAttribute[]): Observable<RoleAttribute[]> {
    let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this._http.post<RoleAttribute[]>('api/Role/Create', JSON.stringify(Role), options);
  }

  updateRole(role) {

    return this._http.put('api/Role/UpdateRole', role);
  }

  DisableRole(Employee_Id) {
    const params = new HttpParams()
      .set('Employee_Id', Employee_Id);
    return this._http.get('api/Role/DisableRole', { params });

  }
}






