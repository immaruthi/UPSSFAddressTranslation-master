import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../services/CustomerService';
import { Customer } from '../models/Customer';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { ExcelService } from '../services/ExcelExport';
@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
  custs: Customer[] = [];
  cust: Customer[];
  EditRowId: any = '';
  constructor(private customerService: CustomerService, private excelService: ExcelService) { }

  addrowtable() {
    this.custs.push(this.NewCustomer());
  }
  delrowtable() {
    if (this.custs.length > 1) {
      this.custs.pop();
    }
  }


  NewCustomer(): Customer {
    let c = new Customer();
    
    c.cust_code = "";
    c.cust_name = "";
    c.location_id = "";
    c.Project_Id = "";
    c.status = "";
    return c;
  }

  ngOnInit() {
    this.custs.push(this.NewCustomer());
    this.LoadCustomer();
  }
  SaveCustomer() {
    return this.customerService.AddCustomer(this.custs).subscribe(
      result => {
        console.log(result);
      },
      err => {
        console.log(err);
      }
    );
  }

  LoadCustomer() {
    this.customerService.GetAllCustomer()
      .subscribe((data: Customer[]) => {
        this.cust = data;
      });
  }

  edit(val) {
    this.EditRowId = val;
  }
  UpdateCustomer(cu: Customer) {
    return this.customerService.UpdateCustomer(cu).subscribe(
      result => {
        console.log(result);
      },
      err => {
        console.log(err);
      }
    );
  }
  DeleteCustomer(cust_code: string) {
    return this.customerService.DeleteCustomer(cust_code).subscribe(
      result => {
        console.log(result);
        this.LoadCustomer();
      },
      err => {
        console.log(err);
      }
    );
  }
  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.cust, 'Customers');
  }
}
