import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';
import { AssignProject } from '../models/AssignProject';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class AssignProjectService {
  constructor(private http: HttpClient) { }

  GetAllAssignProject() {
    return this.http.get<AssignProject[]>(`api/AssignProject/getallassignProject`)
  }

  getassignProjectById(project_Assign_ID: string) {
    const params = new HttpParams()
      .set('project_Assign_ID', project_Assign_ID);
    return this.http.get<AssignProject[]>(`api/AssignProject/getassignProjectbyid` + { params })
  }

  AddAssignProject(assignProject: AssignProject[]): Observable<AssignProject[]> {

    let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.post<AssignProject[]>('api/AssignProject/AddAssignProject', JSON.stringify(assignProject), options);

  }
  UpdateAssignProject(assignProject) {

    let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.put('api/assignProject/UpdateAssignProject', assignProject);

  }
  DeleteAssignProject(project_Assign_ID: string) {

    const params = new HttpParams()
      .set('project_Assign_ID', project_Assign_ID);
    return this.http.get('api/AssignProject/DeleteAssignProject', { params });
  }

}
