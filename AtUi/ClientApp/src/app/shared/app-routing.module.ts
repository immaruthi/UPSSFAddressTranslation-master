import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

/* Component Section */
import { LoginlayoutComponent } from '../layout/loginlayout/loginlayout.component';
import { LoginComponent } from '../login/login.component';
import { AuthGuard } from '../services/AuthGuard';
import { LoginGuard } from '../services/LoginGuard';
import { ApplayoutComponent } from '../layout/applayout/applayout.component';
import { WorkflowComponent } from '../workflow/workflow.component';
import { ShipmentComponent } from '../shipping-data/shipment.component';
import { AdminconfigComponent } from '../adminconfig/adminconfig.component';
const routes: Routes = [
  
  {
    path: '',
    component: LoginlayoutComponent,
    children: [
      { path: '', canActivate: [LoginGuard], component: LoginComponent, pathMatch: 'full' },
    ]
  },
  {
    path: '',
    component: ApplayoutComponent,
    children: [
      { path: 'workflow', canActivate: [AuthGuard], component: WorkflowComponent },
      { path: 'shipment/:WorkflowID', canActivate: [AuthGuard], component: ShipmentComponent },
      { path: 'config', canActivate: [AuthGuard], component: AdminconfigComponent },
      { path: 'home', canActivate: [AuthGuard], component: WorkflowComponent }
    ]
  },
  //{path: 'notauthorized', component: NotauthorizedComponent},
  //{path: '**', component: NotauthorizedComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
