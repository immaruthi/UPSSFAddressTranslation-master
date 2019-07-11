import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { RouterModule } from '@angular/router';
import { EmployeeService } from '../app/services/EmployeeService';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { TopComponent } from './top/top.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { DropdownDirective } from './shared/dropdown.directive';
import { ProjectService } from './services/ProjectService';
import { LoginlayoutComponent} from './layout/loginlayout/loginlayout.component';
import { ApplayoutComponent } from './layout/applayout/applayout.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserService } from './services/UserService';
import { AuthGuard } from './services/AuthGuard';
import { LoginGuard } from './services/LoginGuard';
import { AssignProjectService } from './Services/AssignProjectService';
import { DataService } from './services/data.service';

import { LoaderInterceptor } from "./shared/loader/loaderIntercepter";
import { HomeService } from './services/HomeService';
import { MaterialModule } from './shared/MaterialModule'
import { AdminconfigComponent } from './adminconfig/adminconfig.component';
import { AlertDialogComponent } from './dialogs/alert-dialog/alert-dialog.component';
import { SummaryDialogComponent } from './dialogs/summary-dialog/summary-dialog.component';
import { DialogService } from './services/dialog.service';
import { ExcelService } from './services/ExcelExport';
import { ShippingService } from './services/shipping.service';
import { ShipperListService } from './services/ShipperListService';
import { AddressBookService } from './services/AddressBookService';
import { HttpService } from './shared/http.service';

import { WorkflowComponent } from './workflow/workflow.component';
import { WorkflowService } from './services/WorkflowService';
import { ShipmentComponent } from './shipping-data/shipment.component';

/* Modules Import */
import { AppRoutingModule } from './shared/app-routing.module';
import { UploadedDataComponent } from './shipping-data/uploaded-data/uploaded-data.component';

/*Shared Component*/

import { AlertDialog, ConfirmDialog, ConfirmPopupComponent } from './shared/confirm-popup/confirm-popup.component';
import { LoaderComponent } from './shared/loader/loader.component';
import { LoaderService } from './shared/loader/loader.service';
import { NotificationService } from './services/NotificationService';
/* External Modules */
import { AgGridModule } from 'ag-grid-angular';
import { SentToSfComponent } from './shipping-data/sent-to-sf/sent-to-sf.component';
import { CompletedComponent } from './shipping-data/completed/completed.component';
import { TranslateComponent } from './shipping-data/translate/translate.component';
import { EditableComponent } from './shared/editable/editable.component';
import { EditModeDirective } from './shared/editable/edit-mode.directive';
import { ViewModeDirective } from './shared/editable/view-mode.directive';
import { EditOnEnterDirective } from './shared/editable/edit-on-enter.directive';
import { AddressEditModelComponent } from './shipping-data/address-edit-model/address-edit-model.component';
import { DatePipe } from '@angular/common';
import { ConfirmationDialogComponent } from './dialogs/confirmation-dialog/confirmation-dialog.component';
import { ShipperListComponent } from './shipper-list/shipper-list.component';
import { AddressBookComponent } from './address-book/address-book.component';
import { AddressBookEditModelComponent } from './address-book/address-book-edit-model/address-book-edit-model.component'
import { LogGridComponent } from './log-grid/log-grid.component';
import { AuditingLogComponent } from './auditing-log/auditing-log.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TopComponent,
    SidebarComponent,
    DropdownDirective,
    HomeComponent,
    LoginlayoutComponent,
    ApplayoutComponent,
    LoginComponent,
    AdminconfigComponent,
    AlertDialogComponent,
    SummaryDialogComponent,
    WorkflowComponent,
    ShipmentComponent,
    UploadedDataComponent,
    SentToSfComponent,
    CompletedComponent,
    TranslateComponent,
    EditableComponent,
    EditModeDirective,
    ViewModeDirective,
    EditOnEnterDirective,
    LoaderComponent,
    AlertDialog,
    ConfirmDialog,
    ConfirmPopupComponent,
    AddressEditModelComponent,
    ConfirmationDialogComponent,
    ShipperListComponent,
    AddressBookComponent,
    AddressBookEditModelComponent,
    LogGridComponent,
    AuditingLogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    AppRoutingModule,
    AgGridModule.withComponents([])
  ],
  providers: [
    WorkflowService, EmployeeService,
    ProjectService, UserService,
    AuthGuard, LoginGuard,
    AssignProjectService, ExcelService, HomeService,
    DialogService,
    ShippingService,
    ShipperListService,
    AddressBookService,
      HttpService,
    LoaderService,
    DatePipe,
    NotificationService,
    DataService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoaderInterceptor,
      multi: true
    }
    ],
  bootstrap: [AppComponent],
  entryComponents: [LoginComponent, AlertDialogComponent, SummaryDialogComponent, ConfirmationDialogComponent, AddressEditModelComponent,
    AddressBookEditModelComponent]

})
export class AppModule { }
