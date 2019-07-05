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
    const data = { USR_ID_TE: userId, USR_PWD_TE: password };
    return this.httpService.makePostRequest(`api/Login/ValidateUser`, data)
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
}
