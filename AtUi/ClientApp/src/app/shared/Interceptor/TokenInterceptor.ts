import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs";
import { Router } from "@angular/router";


export class TokenInterceptor implements HttpInterceptor {
constructor() {}

intercept(httpReq: HttpRequest<any>, next: HttpHandler): 
  Observable<HttpEvent<any>> {
  let headers = httpReq.headers;
  if (headers.get('fileupload') !== 'fileupload') {
    headers = httpReq.headers
      .set('Content-Type', 'application/json');
  }
  
 if (headers.get('noToken') === 'noToken') {
   headers = headers.delete('Authorization').delete('noToken');
  }
  
 const newReq = httpReq.clone({headers: headers});

 return next.handle(newReq);
 }
}
