import { Component, OnInit } from '@angular/core';
import { HomeService } from '../services/HomeService';




@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  empcount: number;
  empBenchcount: number;
  empProjcount: number;
  projcount: number;
  custcount: number;

  constructor(private homeservice: HomeService) { }

  ngOnInit() {
    this.LoadEmployeeCount();
  }

  LoadEmployeeCount() {
    this.homeservice.getEmployeeCount()
      .subscribe((data: number) => {
        this.empcount = data;
        this.LoadEmpProjCount();
      });
  }
  LoadEmpBenchCount() {
    this.homeservice.getEmpBenchCount()
      .subscribe((data: number) => {
        this.empBenchcount = data;
        this.LoadCustCount();
      });
  }
  LoadEmpProjCount() {
    this.homeservice.getEmpProjCount()
      .subscribe((data: number) => {
        this.empProjcount = data;
        this.LoadEmpBenchCount();
      });
  }
  LoadProjectCount() {
    this.homeservice.getProjCount()
      .subscribe((data: number) => {
        this.projcount = data;
      });
  }
  LoadCustCount() {
    this.homeservice.getCustCount()
      .subscribe((data: number) => {
        this.custcount = data;
        this.LoadProjectCount();
      });
  }
}
