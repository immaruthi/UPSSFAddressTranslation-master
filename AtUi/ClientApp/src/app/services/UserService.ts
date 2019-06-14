import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable()
export class UserService {
  constructor(private httpClient: HttpClient, private router: Router) { }

  ValidateUser(userId: any, password: any) {
    const params = new HttpParams()
      .set('userId', userId)
      .set('password', password);
    return this.httpClient.get(`api/Login/ValidateUser`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUser`, { params })
  }

  ValidateUserId(userId: any) {
    const params = new HttpParams()
      .set('userId', userId)

    return this.httpClient.get(`api/Login/ValidateUserId`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUserId`, { params })

  }
  getLoginData(userId: any) {
    const params = new HttpParams()
      .set('Emp_Id', userId)

    return this.httpClient.get(`api/Login/getLoginData`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/getLoginData`, { params })

  }

  InsertUser(firstName: any, lastName: any, userName: any, password: any) {
    const params = new HttpParams()
      .set('firstName', firstName)
      .set('lastName', lastName)
      .set('userName', userName)
      .set('password', password);
    return this.httpClient.get(`api/Login/InsertUser`, { params })
    //return this.httpClient.get(`https://localhost:44330/api/values/ValidateUser`, { params })
  }


  //logout service method
  logout() {
    localStorage.removeItem("Emp_Id");
    localStorage.removeItem("pwd");
    


  }


}
