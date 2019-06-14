import { Component, OnInit } from '@angular/core';
import { ProjectAttribute } from '../models/ProjectAttribute';
import { ProjectService } from '../services/ProjectService';
import { ExcelService } from '../services/ExcelExport';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map'; 

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {

  constructor(private projectservice: ProjectService, private excelService: ExcelService) {

  }
  projectlist: ProjectAttribute[];
  proj: ProjectAttribute[] = [];


  addrowtable() {
    this.proj.push(this.NewProject());
  }
  delrowtable() {
    if (this.proj.length > 1) {
      this.proj.pop();
    }
  }


  NewProject(): ProjectAttribute {
    let pr = new ProjectAttribute();
    pr.Project_ID = "";
    pr.Project_Name = "";
    pr.Project_Description = "";
    pr.Project_StartDate = "";
    pr.Project_EndDate = "";

    return pr;
  }

  ngOnInit() {
    this.proj.push(this.NewProject());
    this.LoadProjectDetails();
  }



  OnSubmit() {
    var ans = confirm("Do you want to Add a New Record");

    if (ans) {
      return this.projectservice.AddProjects(this.proj).subscribe(
        result => {
          console.log(result);
          this.proj = [];
          this.proj.push(this.NewProject());
          this.LoadProjectDetails();

        },
        err => {
          console.log(err);
        }
      );
    }
  }

  LoadProjectDetails() {
    this.projectservice.getProjects()
      .subscribe((data: ProjectAttribute[]) => {
        this.projectlist = data;
      });

  }
  EditRowId: any = '';

  edit(val) {
    this.EditRowId = val;
  }
  update(pr: ProjectAttribute) {
    var ans = confirm("Do you want to save the changes" + pr);
    if (ans) {
      return this.projectservice.UpdateProject(pr).subscribe(
        result => {
          console.log(result);
        },
        err => {
          console.log(err);
        }
      );
    }
  }

  cancel(val) {
    this.EditRowId = !this.EditRowId
  }

  disable(Project_ID: string) {
    var ans = confirm("Do you want to delete the record: " + Project_ID);
    if (ans) {
      return this.projectservice.DisableProject(Project_ID).subscribe(result => {
        console.log(result);
        this.LoadProjectDetails();
      },
        err => {
          console.log(err);
        }
      );

    }
  }
  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.projectlist, 'Projects');
  }
}
