import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {
  
  //private LOCAL_API_URL = window.location.origin + '/';
  private LOCAL_API_URL = 'https://addresstranslation.azurewebsites.net/';
  
  constructor(private _http: HttpClient) {
    if (location.hostname == "localhost") {
      this.LOCAL_API_URL = 'https://localhost:44330/';
    }
  }
  
  /**
   * Make a GET request
   * @param url
   * @param params
   * @returns {Observable<any>}
   */
  makeGetRequest(url: string, params?: any): Observable<any> {
    
    return this._http.get(this.LOCAL_API_URL + url, {params: params});
  }
  
  /**
   * Make a POST request
   * @param url
   * @param body
   * @returns {Observable<any>}
   */
  makePostRequest(url: string, body?: any) {
    
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._http.post(this.LOCAL_API_URL + url, body, {headers: headers});
  }
  
  /**
   * Make a PUT request
   * @param {string} url
   * @param body
   */
  makePutRequest(url: string, body?: any) {
    
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._http.put(this.LOCAL_API_URL + url, body, {headers: headers});
  }
  
  /**
   * Make a PUT request
   * @param {string} url
   * @param body
   */
  makeDeleteRequest(url: string, body?: any) {
    return this._http.delete(this.LOCAL_API_URL + url, body);
  }
}
