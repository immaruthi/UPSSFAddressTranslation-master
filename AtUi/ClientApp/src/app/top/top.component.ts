import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/UserService';
import { AuthenticationService } from '../services/authentication.service';
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
  userName: string;
  loginfo: string;
  isHandset$: any;
  role: string;
    constructor(private _authService: AuthenticationService,private userService: UserService, private router: Router) { }
  log_info: LoginData;
  logout() {
    this._authService.logout();
    this.router.navigate(['']);
  }
  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    let jwt = currentUser.token;
    let jwtData = jwt.split('.')[1]
    let decodedJwtJsonData = window.atob(jwtData)
    let decodedJwtData = JSON.parse(decodedJwtJsonData);
    this.role = decodedJwtData.roles;
    this.userName = currentUser != null ? currentUser.UserName : '';
  }
}
