import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { HttpService } from '../shared/http.service';


@Injectable()
export class LogFilesService {

  constructor(private httpClient: HttpClient, private router: Router,
    private httpService: HttpService) { }

  public GetLogFilesList() {
    return this.httpService.makeGetRequest('api/LogFile/GetLogFiles');
  }

  public GetLogGrid(filename) {
    return this.httpService.makeGetRequest('api/LogFile/ReadFileData?filePath=' + filename);
  }

}
