import { Component, OnInit } from '@angular/core';
import { Employee } from '../models/Employee';
import { EmployeeService } from '../services/EmployeeService';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { ExcelService } from '../services/ExcelExport';
import { Designation } from '../models/Designation';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { Department } from '../models/Department';
import { EdgePractice } from '../models/EdgePractice';
import { CoeDescription } from '../models/CoeDescription';
import { BaseLocation } from '../models/BaseLocation';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {
  emps: Employee[];//var
  employee: Employee[];//var
  design: Designation[];//var
  dept: Department[];//var
  edge: EdgePractice[];//var
  coe: CoeDescription[];//var
  Location: BaseLocation[];//var
  EditRowId: any = '';//var
  EmployeeForm: FormGroup;//var

  constructor(private employeeService: EmployeeService, private excelService: ExcelService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.EmployeeForm = new FormGroup({
      emp_Id: new FormControl('', [Validators.required, Validators.maxLength(8), Validators.minLength(8)]),
      emp_Name: new FormControl('', [Validators.required, Validators.pattern('[a-zA-Z]+$')]),
      designation_Id: new FormControl('', [Validators.required]),
      department_Id: new FormControl('', [Validators.required]),
      edge_Practice_Id: new FormControl('', [Validators.required]),
      coe_Id: new FormControl('', [Validators.required]),
      location_Code: new FormControl('', [Validators.required]),
      joining_Date: new FormControl('', [Validators.required]),
      contact_Number: new FormControl('', [Validators.required, Validators.maxLength(10), Validators.minLength(10), Validators.pattern('[0-9]+$')]),
      email_Id: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9_.+-]+@[pactera.com]+$')]),
      reporting_To: new FormControl('', [Validators.required]),
      reporting_To_Email: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9_.+-]+@[pactera.com]+$')]),
    });
    this.LoadEmployeeDetails('');
    this.LoadDesignationDescription();
    this.LoadDepartmentDescription();
    this.LoadEdgePracticeDescription();
    this.LoadGetAllCoeDescription();
    this.LoadGetAllCity();
  }

  //To Retrieve all the Employee details
  LoadEmployeeDetails(Emp_ID: string) {
    this.employeeService.GetAllEmployee(Emp_ID)
      .subscribe((data: Employee[]) => {
        this.employee = data;
      });
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.EmployeeForm.controls[controlName].hasError(errorName);
  }

  //To add the Employee Details
  AddEmployee()
  {
    if (this.EmployeeForm.valid) {
      this.emps = Object.assign({}, this.EmployeeForm.value);
      return this.employeeService.AddEmployee(this.emps).subscribe(
      
        result => {
          console.log(result);
          this.LoadEmployeeDetails('');
        },
        err => {
          console.log(err);
        }
        
      );
     
    }
  }

  //To get the dropdown for EmployeeDesignation
  LoadDesignationDescription()
  {
    this.employeeService.GetAllDesignation()
      .subscribe((data: Designation[]) => {
        this.design = data;
      });
  }
  //To get the dropdown for Department
  LoadDepartmentDescription() {
    this.employeeService.GetAllDepartment()
      .subscribe((data: Department[]) => {
        this.dept = data;
      })
  }

  //To get the dropdown for EDGE
  LoadEdgePracticeDescription() {
    this.employeeService.GetAllEdgePractice()
      .subscribe((data: EdgePractice[]) => {
        this.edge = data;
      })
  }

  //To get the dropdown for COE
  LoadGetAllCoeDescription() {
    this.employeeService.GetAllCoeDescription()
      .subscribe((data: CoeDescription[]) => {
        this.coe = data;
      })
  }

  //To get the dropdown for Location
  LoadGetAllCity() {
    this.employeeService.GetAllCity()
      .subscribe((data: BaseLocation[]) => {
        this.Location = data;
      })
  }

  //To update the Employee Details
  updateEmployee(emp_Id: string)
  {
  
    return this.employeeService.updateEmployee(emp_Id).subscribe(
      result => {
        console.log(result);
        this.LoadEmployeeDetails(''); // load employee
      },
      err => {
        console.log(err);
      }
    );
  }
  editRow_Click(val) {
    this.EditRowId = val;
  }
  cancel_Click(ep: Employee) {
    var ans = confirm("Do you want to cancel the changes: " + ep);
    if (ans) {
      this.EditRowId = !this.EditRowId;
      this.LoadEmployeeDetails('');
    } err => {
      console.log(err);
    }
  }

  // Delete Employee Details
  delete_Click(Emp_Id: string) {
    var ans = confirm("Do you want to delete the record: " + Emp_Id);
    if (ans) {
      return this.employeeService.DeleteEmployee(Emp_Id).subscribe(result => {
        console.log(result);
        this.LoadEmployeeDetails('');
      },
        err => {
          console.log(err);
        }
      );
    }
  }

  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.employee, 'Employee');
  }

}

