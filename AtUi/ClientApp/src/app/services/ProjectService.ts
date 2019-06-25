import { Injectable} from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { ProjectAttribute } from '../Models/ProjectAttribute';

@Injectable()
export class ProjectService {
  myAppUrl: string = "";
  constructor(private http: HttpClient) {

  }
  getProjects() {
    return this.http.get('api/Projects/GetAllProjects')
  }
  AddProjects(project: ProjectAttribute[]): Observable<ProjectAttribute[]> {
    let httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json');
    let options = { headers: httpHeaders };
    return this.http.post<ProjectAttribute[]>('api/Projects/AddProjects', JSON.stringify(project), options);

  }
UpdateProject(project) {
    return this.http.put('api/Projects/UpdateProject', project);
  }
DisableProject(Project_ID) {
    const params = new HttpParams()
      .set('Project_ID', Project_ID);
    return this.http.get('api/Projects/DisableProject', { params });

  }
}
