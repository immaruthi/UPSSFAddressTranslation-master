import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/UserService';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { LoginData } from '../models/LoginData';

@Component({
  selector: 'app-top',
  templateUrl: './top.component.html',
  styleUrls: ['./top.component.css']
})
export class TopComponent implements OnInit {
  employeeId: string;
  loginfo: string;
constructor(private userService: UserService, private router: Router) { }
  log_info: LoginData;
  logout() {
    this.userService.logout();
    this.router.navigate(['']);
  }
  ngOnInit() {
    this.userService.getLoginData(localStorage.getItem("Emp_Id"))
      .subscribe((data: LoginData) => {
        this.log_info = data;
        this.employeeId = this.log_info.emp_Id;
        this.loginfo = this.log_info.last_Login_Date;
      });
  }
}
