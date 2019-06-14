import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';
import { Workflow } from '../models/Workflow';
import { Observable } from 'rxjs/Observable';
import { Designation } from '../models/Designation';
import { Department } from '../models/Department';
import { EdgePractice } from '../models/EdgePractice';
import { CoeDescription } from '../models/CoeDescription';
import { BaseLocation } from '../models/BaseLocation';


@Injectable()
export class WorkflowService {
  constructor(private http: HttpClient) { }
  GetAllWorkflow(ID: string) {
    const params = new HttpParams()
      .set('ID', ID);
    return this.http.get<Workflow[]>(`api/Workflow/getallworkflow`, { params })
  }
  AddWorkflow(workflow: Workflow[]): Observable<Workflow[]> {
    let httpHeaders = new HttpHeaders().set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.post<Workflow[]>('api/Workflow/AddWorkflow', JSON.stringify(workflow), options);
  }
}




