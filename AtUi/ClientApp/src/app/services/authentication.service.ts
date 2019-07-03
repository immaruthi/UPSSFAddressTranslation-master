import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { environment } from '../../environments/environment';

@Injectable()
export class AuthenticationService {

  //variable initialization
  _token: any;
  _baseURL: string =  environment.LOCAL_API_URL;;

  constructor(private _http: HttpClient) {
    var currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
    this._token = currentUser && currentUser.token;
  }
  login(userId: string, password: string): Observable<boolean> {
    debugger;
    let headers = new HttpHeaders();
    headers = headers.append('noToken', 'noToken');
    let userData = { USR_ID_TE: userId, USR_PWD_TE: password };
    return this._http.post(this._baseURL + 'api/Login', userData, { headers: headers })
      .map(
        (response: any) => {
        // login successfully, if there is a jwt token in the response
        let token = response.token;
        if (token) {
          //set the token property for validate token in the app.
          this._token = token;

          //store username and jwt token in local storage to keep user logged in between page refreshes.
          this.SetCurrentUserSession(response);

          //returns true to indicate successful login
          return true;
        }
        else {
          // returns false to indicate failed login
          return false;
        }
      },
      (error:any) =>
      {
        if (error.statusCode == 401) {
          return false;
        }
        console.log(error)
      });
  }

  private SetCurrentUserSession(response: any) {
      sessionStorage.setItem('currentUser', JSON.stringify({
          token: response.token,
          expiration: response.expiration,
          UserId: response.userId,
          UserName: response.userName
      }));
  }

  logout(): void {
    // clear token remove user from local storage to log user out.
    this._token = null;
    sessionStorage.removeItem('currentUser');
  }
}
