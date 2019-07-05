import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpParams } from '@angular/common/http';
import { Workflow } from '../models/Workflow';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';
import { HttpService } from '../shared/http.service';


@Injectable()
export class WorkflowService {
  constructor(private httpClient: HttpClient, private httpService: HttpService) { }
  GetAllWorkflow() {
    return this.httpService.makeGetRequest(`api/Workflow/GetAll`);
  }
  AddWorkflow(workflow: Workflow[]): Observable<Workflow[]> {
    return this.httpClient.post<Workflow[]>('api/Workflow/AddWorkflow', JSON.stringify(workflow));
  }

  UploadFile(fileToUpload: File): Observable<Object> {
    const endpoint = 'api/Shipment/ExcelFileUpload/';
    let  formData: FormData = new FormData();

    formData.append('excelFileName', fileToUpload, fileToUpload.name);
    return this.httpService.makePostRequestforFormData(endpoint, formData)
      .map((response: Response) => {
        console.log(response);
        return response;
      });
  }
}




