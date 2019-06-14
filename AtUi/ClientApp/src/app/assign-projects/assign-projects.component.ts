import { Component, OnInit } from '@angular/core';
import { AssignProjectService } from '../Services/AssignProjectService';
import { AssignProject } from '../models/AssignProject';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { ExcelService } from '../services/ExcelExport';



@Component({
  selector: 'app-assign-projects',
  templateUrl: './assign-projects.component.html',
  styleUrls: ['./assign-projects.component.css']
})
export class AssignProjectsComponent implements OnInit {

  EditRowId: any = '';
  rows = [];
  constructor(private AssignProjectService: AssignProjectService, private excelService: ExcelService) { }

 assignproj: AssignProject[]=[];
 public Pro: AssignProject[]=[];
 
  addrowtable() {
    this.Pro.push(this.NewAssignProject());
  }

  removerowtable() {
    if (this.Pro.length > 1) {
      this.Pro.pop();
    }
    
  }
  NewAssignProject(): AssignProject {
    let ap = new AssignProject();
    ap.Emp_Id = "";
    ap.Project_ID = "";
    ap.Assign_Project_StartDate = "";
    ap.Assign_Project_EndDate = "";
    ap.Billable = "";
    ap.Billing_Percentage = "";
    ap.Location = "";
    ap.Onsite = "";
    return ap;
  }
  ngOnInit() {
    this.Pro.push(this.NewAssignProject());
    this.LoadAssignProject();

  }
  OnSubmit() {
    return this.AssignProjectService.AddAssignProject(this.Pro).subscribe(
      result => {
        console.log(result);
        this.assignproj = [];
        this.LoadAssignProject();
      },
      err => {
        console.log(err);
      }
    );
  }

  LoadAssignProject() {
    this.AssignProjectService.GetAllAssignProject()
      .subscribe((data: AssignProject[]) => {
        this.assignproj = data;
      });
  }
  edit(val) {
    this.EditRowId = val;
  }


  update(assignproj: AssignProject) {
    return this.AssignProjectService.UpdateAssignProject(assignproj).subscribe(
      result => {
        console.log(result);
      },
      err => {
        console.log(err);
      }
    );
  }


  cancel(assignproj: AssignProject) {
    var ans = confirm("Do you want to cancel the changes: " + assignproj);
    if (ans) {
      this.EditRowId = !this.EditRowId
    } err => {
      console.log(err);
    }
  }


  Delete(project_Assign_ID: string) {

    var ans = confirm("Do you want to delete the record: " + project_Assign_ID);
    if (ans) {
      return this.AssignProjectService.DeleteAssignProject(project_Assign_ID).subscribe(
        result => {
          console.log(result);
          this.LoadAssignProject();
        },
        err => {
          console.log(err);
        }
      );
    }
  }
  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.assignproj, 'AssignProject');
  }
}

