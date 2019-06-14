import { Component, OnInit } from '@angular/core';
import { RoleAttribute } from '../models/RoleAttribute';
import { RoleService } from '../services/RoleService';
import { ExcelService } from '../services/ExcelExport';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map'; 
import { parse } from 'querystring';
import { EmpDropAttribute } from '../models/empDropAttribute';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {
  constructor(private roleservice: RoleService, private excelService: ExcelService) { }

  roles: RoleAttribute[] = [];
  empdroplist: EmpDropAttribute[];
  public RoleAttributeList: RoleAttribute[];
  EditRowId: any = '';
  addrowtable() {
    this.roles.push(this.NewRole());
  }
  delrowtable() {
    if (this.roles.length>1)
    this.roles.pop();
  }
  NewRole(): RoleAttribute {
    let r = new RoleAttribute();
    r.Employee_Id = "";
    r.Employee_Name = "";
    r.Role_Designation = "";
    r.Role_Description = "";
    r.Role_Status = "";
    r.Role_StartDate = new Date("");
    r.Role_EndDate = new Date("");
    return r;
  }
  ngOnInit() {
    this.roles.push(this.NewRole());
    this.LoadRoleDetails();
    this.LoadEmpDrop();

  }
  OnSubmit() {
    return this.roleservice.saveRole(this.roles).subscribe(
      result => {
        console.log(result);
      },
      err => {
        console.log(err);
      }
    );
  }
  edit(val) {
    this.EditRowId = val;
  }
  cancel(role: RoleAttribute) {
    var ans = confirm("Do you want to cancel the changes: " + role);
    if (ans) {
      this.EditRowId = !this.EditRowId;
    } err => {
      console.log(err);
    }
  }
  update(role: RoleAttribute) {
    return this.roleservice.updateRole(role).subscribe(
      result => {
        console.log(result);
      },
      err => {
        console.log(err);
      }
    );
  }
  LoadRoleDetails() {
    this.roleservice.getRole()
      .subscribe((data: RoleAttribute[]) => {
        this.RoleAttributeList = data;
      });
  }
  LoadEmpDrop() { this.roleservice.getempdrop().subscribe((data: EmpDropAttribute[]) => { this.empdroplist = data; }); }
  delete(Employee_Id: string) {
    var ans = confirm("Do you want to delete the record: " + Employee_Id);
    if (ans) {
      return this.roleservice.DisableRole(Employee_Id).subscribe(result => {
        console.log(result);
        this.LoadRoleDetails();
      },
        err => {
          console.log(err);
        }
      );


    }
  }
  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.RoleAttributeList, 'Roles');
  }
}
