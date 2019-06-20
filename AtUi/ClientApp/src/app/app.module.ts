import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { EmployeeService } from '../app/services/EmployeeService';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { TopComponent } from './top/top.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { DropdownDirective } from './shared/dropdown.directive';
import { AssignProjectsComponent } from './assign-projects/assign-projects.component';
import { EmployeeComponent } from './employee/employee.component';
import { ProjectsComponent } from './projects/projects.component';
import { RolesComponent } from './roles/roles.component';
import { ProjectService } from './services/ProjectService';
import { RoleService } from './services/RoleService';
import { LoginlayoutComponent} from './layout/loginlayout/loginlayout.component';
import { ApplayoutComponent } from './layout/applayout/applayout.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserService } from './services/UserService';
import { AuthGuard } from './services/AuthGuard';
import { LoginGuard } from './services/LoginGuard';
import { CustomersComponent } from './customers/customers.component';
import { CustomerService } from './services/CustomerService';
import { AssignProjectService } from './Services/AssignProjectService';

import { HomeService } from './services/HomeService';
import { MaterialModule } from './shared/MaterialModule'
import { AdminconfigComponent } from './adminconfig/adminconfig.component';
import { AlertDialogComponent } from './dialogs/alert-dialog/alert-dialog.component';
import { DialogService } from './services/dialog.service';
import { ExcelService } from './services/ExcelExport';

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
/* External Modules */
import { AgGridModule } from 'ag-grid-angular';
import { SentToSfComponent } from './shipping-data/sent-to-sf/sent-to-sf.component';
import { TranslateComponent } from './shipping-data/translate/translate.component';
import { EditableComponent } from './shared/editable/editable.component';
import { EditModeDirective } from './shared/editable/edit-mode.directive';
import { ViewModeDirective } from './shared/editable/view-mode.directive';
import { EditOnEnterDirective } from './shared/editable/edit-on-enter.directive';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TopComponent,
    SidebarComponent,
    DropdownDirective,
    HomeComponent,
    AssignProjectsComponent,
    EmployeeComponent,
    ProjectsComponent,
    RolesComponent,
    LoginlayoutComponent,
    ApplayoutComponent,
    LoginComponent,
    CustomersComponent,
    AdminconfigComponent,
    AlertDialogComponent,
    WorkflowComponent,
    ShipmentComponent,
    UploadedDataComponent,
    SentToSfComponent,
    TranslateComponent,
    EditableComponent,
    EditModeDirective,
    ViewModeDirective,
    EditOnEnterDirective,
    LoaderComponent,
    AlertDialog,
    ConfirmDialog,
    ConfirmPopupComponent
   
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
    ProjectService, RoleService, UserService,
    AuthGuard, CustomerService, LoginGuard,
    AssignProjectService, ExcelService, HomeService,
    DialogService,
    LoaderService],
  bootstrap: [AppComponent],
  entryComponents: [LoginComponent, AlertDialogComponent]

})
export class AppModule { }
