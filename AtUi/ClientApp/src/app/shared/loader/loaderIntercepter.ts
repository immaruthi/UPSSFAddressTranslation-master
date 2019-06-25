import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { LoaderService } from "./loader.service";

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  private requests: HttpRequest<any>[] = [];
  
  constructor(private loaderService: LoaderService) {
  }
  
  removeRequest(req: HttpRequest<any>) {
    const i = this.requests.indexOf(req);
    if (i !== -1) {
      this.requests.splice(i, 1);
      this.loaderService.isLoading.next(this.requests.length > 0);
    }
  }
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.requests.push(req);
    this.loaderService.isLoading.next(true);
    return Observable.create(observer => {
      const subscription = next.handle(req)
        .subscribe(
          event => {
            if (event instanceof HttpResponse) {
              observer.next(event);
              this.removeRequest(req);
            }
          },
          err => {
            observer.error(err);
            this.removeRequest(req);
            
          },
          () => {
            // this.removeRequest(req);
            observer.complete();
          });
      // teardown logic in case of cancelled requests
      return () => {
        this.removeRequest(req);
        subscription.unsubscribe();
      };
    });
  }
}
