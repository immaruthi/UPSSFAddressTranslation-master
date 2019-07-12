import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';

@Component({
  selector: 'app-log-grid',
  templateUrl: './log-grid.component.html',
  styleUrls: ['./log-grid.component.css']
})





export class LogGridComponent implements OnInit {

  logdata = [
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Hydrogen', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Helium', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Lithium', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Beryllium', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Boron', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Carbon', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Nitrogen', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Oxygen', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Fluorine', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
    { logDate: '2019-07-10T22:14:23.4497056+05:30', userId: 'Neon', apiType: 4, logRequest: 'Test Log Request', logResponse: 'Test Lod Response', logException: 'System.Exception', message: 'Test Message, Data: null, InnerException: null, HelpURL: null, StackTraceString: null, RemoteStackTraceString: null, RemoteStackIndex: 0, ExceptionMethod: null, HResult: -2146233088, Source: null, WatsonBuckets: null' },
  ];

  displayedColumns: string[] = ['logDate', 'userId', 'apiType', 'logRequest', 'logResponse','logException', 'message'];
  dataSource = new MatTableDataSource<any>();
  
  constructor() { }

  @ViewChild(MatPaginator) paginator: MatPaginator;

  ngOnInit() {
    this.dataSource.data = this.logdata;
    this.dataSource.paginator = this.paginator;

  }
 
}


