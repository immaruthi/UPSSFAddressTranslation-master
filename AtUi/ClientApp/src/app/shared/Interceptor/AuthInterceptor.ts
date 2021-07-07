import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs";
import { Router } from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router) {

  }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (request.headers.get('notoken') !== 'noToken') {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));

      if (currentUser != null && currentUser.token) {
        let jwt = currentUser.token;
      let jwtData = jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData);
      let roles = decodedJwtData.roles; 

        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`,

          }
        });
      }
      
    }
    return next.handle(request).do((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          localStorage.removeItem('currentUser');
          this.router.navigate(['/login']);
        }
      }
    });
  }
}
