import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { List } from 'linq-typescript';
import { HttpService } from '../shared/http.service';
import { environment } from '../../environments/environment';
 
@Injectable()
export class UserService {
  constructor(private httpClient: HttpClient, private router: Router,
              private httpService: HttpService) { }

  ValidateUser(userId: any, password: any) {
    const params = new HttpParams()
      .set('userId', userId)
      .set('password', password);
    //return this.httpService.makeGetRequest('api/Login/ValidateUser', { params })
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient.get(environment.LOCAL_API_URL + `api/Login/ValidateUser`, { params })
    //const data = { userId: userId, password: password };
    //return this.httpService.makePostRequest(`api/Login/ValidateUser`, data)
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUser`, { params })
  }

  ValidateUserId(userId: any) {
    const params = new HttpParams()
      .set('userId', userId)

    return this.httpClient.get(environment.LOCAL_API_URL + `api/Login/ValidateUserId`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUserId`, { params })

  }
  getLoginData(userId: any) {
    const params = new HttpParams()
      .set('Emp_Id', userId)

    return this.httpClient.get(environment.LOCAL_API_URL + `api/Login/getLoginData`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/getLoginData`, { params })

  }

  InsertUser(firstName: any, lastName: any, userName: any, password: any) {
    const params = new HttpParams()
      .set('firstName', firstName)
      .set('lastName', lastName)
      .set('userName', userName)
      .set('password', password);
    return this.httpClient.get(environment.LOCAL_API_URL + `api/Login/InsertUser`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUser`, { params })
  }

  getAllWorkflows(user:any) {
    const params = new HttpParams()
      .set('Emp_Id', user)

    return this.httpClient.get(environment.LOCAL_API_URL + `api/ExcelWorkflow/getExcelData`, { params })
  }
  postFile(fileToUpload: File, user: any): Observable<Object> {
    debugger;
    let Emp_Id = user;
    //const endpoint = 'api/ExcelWorkflow/UploadExcel';
    //const endpoint = 'https://atservicetest.azurewebsites.net/api/Shipment/ExcelFileUpload';
    const endpoint = environment.LOCAL_API_URL + 'api/Shipment/ExcelFileUpload';
    const formData: FormData = new FormData();
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    //let options = new RequestOptions({ headers: headers });
    
    formData.append('Emp_ID', user);
    formData.append('excelFileName', fileToUpload, fileToUpload.name);
    let fileList = new List<File>([fileToUpload]);
    var body = 'excelFileName=' + formData + '&Emp_Id= 8';
    return this.httpClient.post(endpoint, formData)
      .map((response: Response) => {
        console.log(response);
        return response;
      });
  }

  //logout service method
  logout() {
    localStorage.removeItem("Emp_Id");
    localStorage.removeItem("pwd");
    


  }


}
