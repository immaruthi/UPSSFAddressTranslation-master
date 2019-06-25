import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';
import { Employee } from '../models/Employee';
import { Observable } from 'rxjs/Observable';
import { Designation } from '../models/Designation';
import { Department } from '../models/Department';
import { EdgePractice } from '../models/EdgePractice';
import { CoeDescription } from '../models/CoeDescription';
import { BaseLocation } from '../models/BaseLocation';


@Injectable()
export class EmployeeService {
  constructor(private http: HttpClient) {  }
  GetAllEmployee(Emp_Id: string) {
    const params = new HttpParams()
      .set('Emp_ID', Emp_Id);
    return this.http.get<Employee[]>(`api/Employee/getallemployee`, { params })
  }
  AddEmployee(employee: Employee[]): Observable<Employee[]> {
    let httpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.post<Employee[]>('api/Employee/AddEmployee', JSON.stringify(employee), options);
  }
  updateEmployee(employee) {
    return this.http.put('api/Employee/UpdateEmployee', employee);

  }
  DeleteEmployee(Emp_Id: string) {
    const params = new HttpParams()
      .set('Emp_Id', Emp_Id);
    return this.http.get('api/Employee/DeleteEmployee', { params });
  }
  GetAllDesignation() {
    
    return this.http.get<Designation[]>('api/Employee/getalldesignation')
  }
  GetAllDepartment() {

    return this.http.get<Department[]>('api/Employee/getalldepartment')
  }

  GetAllEdgePractice() {

    return this.http.get<EdgePractice[]>('api/Employee/getallEdgePractice')
  }

  GetAllCoeDescription() {
    return this.http.get<CoeDescription[]>('api/Employee/getallcoedescription')
  }

  GetAllCity() {
    return this.http.get<BaseLocation[]>('api/Employee/getallcity')
  }
}




