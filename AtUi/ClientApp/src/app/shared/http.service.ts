import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { environment } from '../../environments/environment';

@Injectable()
export class HttpService {

  private LOCAL_API_URL = environment.LOCAL_API_URL;
  //private LOCAL_API_URL = window.location.origin + '/';
  //private LOCAL_API_URL = 'https://atservicetest.azurewebsites.net/';
  
  constructor(private _http: HttpClient) {
    //if (location.hostname == "localhost") {
    //  this.LOCAL_API_URL = 'https://localhost:44330/';
    //}
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
  makePostRequest(url: string, body?: any): Observable<any> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._http.post(this.LOCAL_API_URL + url, body, {headers: headers});
  }
  /**
 * Make a POST request
 * @param url
 * @param body
 * @returns {Observable<any>}
 */
  makePostRequestWithList(url: string, shipmentWorkFlowRequest?: any[]): Observable<any> {
    debugger;
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this._http.post(this.LOCAL_API_URL + url, shipmentWorkFlowRequest)
      .map((response) => {
        console.log(response);
        return response;
      });
  }

  /**
   * Make a XML POST request
   * @param url
   * @param body
   * @returns {Observable<any>}
   */
  makePostRequestXML(url: string, body?: any): Observable<any> {

    const headers = new HttpHeaders().set('Content-Type', 'application/xml');
    return this._http.post(this.LOCAL_API_URL + url, body, { headers: headers });
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
