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
    ShipmentComponent
   
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {
        path: '',
        component: LoginlayoutComponent,
        children: [
          { path: '', canActivate: [LoginGuard],component: LoginComponent, pathMatch: 'full' },
        ]
      },

      // App routes goes here here
      {
        path: '',
        component: ApplayoutComponent,
        children: [
          { path: 'home', canActivate: [AuthGuard], component: HomeComponent },
          { path: 'assignProjects', canActivate: [AuthGuard],component: AssignProjectsComponent },
          { path: 'employee', canActivate: [AuthGuard],component: EmployeeComponent  },
          { path: 'workflow', canActivate: [AuthGuard], component: WorkflowComponent },
          { path: 'shipment', canActivate: [AuthGuard], component: ShipmentComponent },
          { path: 'projects', canActivate: [AuthGuard],component: ProjectsComponent  },
          { path: 'roles', canActivate: [AuthGuard], component: RolesComponent },
          { path: 'customers', canActivate: [AuthGuard], component: CustomersComponent },
          { path: 'config', canActivate: [AuthGuard], component: AdminconfigComponent }
        ]
      },
    ])
  ],
  providers: [WorkflowService, EmployeeService,ProjectService, RoleService, UserService, AuthGuard, CustomerService, LoginGuard, AssignProjectService, ExcelService, HomeService, DialogService],
  bootstrap: [AppComponent],
  entryComponents: [LoginComponent, AlertDialogComponent]

})
export class AppModule { }
