import { Component, ViewChild, OnInit } from '@angular/core';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl, FormArray, FormGroup, Validators } from '@angular/forms';
import { ShippingService } from '../../services/shipping.service';
import { Router, ActivatedRoute, Params } from '@angular/router';


@Component({
  selector: 'app-translate',
  templateUrl: './translate.component.html',
  styleUrls: ['./translate.component.css']
})
export class TranslateComponent implements OnInit {
  displayedColumns =
    //['select', 'smT_NR_TE', 'shP_DT',
    //  'shP_CPY_NA', 'shP_ADR_TE', 'shP_ADR_TR_TE', 'orG_CTY_TE', 'orG_PSL_CD', 'rcV_CPY_TE', 'rcV_ADR_TE', 'dsT_CTY_TE',
    //  'dsT_PSL_TE'
    //];
    ['select', 'smT_STA_NR', 'smT_NR_TE', 'shP_DT', 'shP_CPY_NA', 'fsT_INV_LN_DES_TE', 'shP_ADR_TE',
      'shP_ADR_TR_TE', 'shP_CTC_TE', 'shP_PH_TE', 'orG_CTY_TE', 'orG_PSL_CD', 'imP_SLC_TE', 'rcV_CPY_TE',
      'rcV_ADR_TE', 'dsT_CTY_TE', 'dsT_PSL_TE', 'coD_TE'];

  public ResponseData: any[] = [];
  //public dataSource: any[] = [];
  dataSource = new MatTableDataSource<Element>();
  public errorMessage: string;

  //dataSource = new MatTableDataSource<Element>();
  selection = new SelectionModel<any>(true, []);
  constructor(private shippingService: ShippingService, private activatedRoute: ActivatedRoute,
              private router: Router) {

  }

  @ViewChild(MatPaginator) paginator: MatPaginator;



  /**
  * Set the paginator after the view init since this component will
  * be able to query its view for the initialized paginator.
  */
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
       
    const WorkflowID = this.activatedRoute.snapshot.params.WorkflowID;
    if (WorkflowID) {
      this.getTranslateData(WorkflowID);
    }
  }

  getTranslateData(WorkflowID: any) {
    this.ResponseData = [];
    this.shippingService.getTranslateData(WorkflowID).subscribe((response: any) => {
      this.ResponseData = response;
      this.dataSource.data = response;
      this.dataSource.paginator = this.paginator;
    }, error => (this.errorMessage = <any>error));
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  
    masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1}`;
  }

  
  updateField(index, field) {
    debugger;
    const control = this.getControl(index, field);
    //if (control.valid) {
    //  //this.core.update(index, field, control.value);
    //}

  }

  getControl(index, fieldName) {
    debugger;
    //const a = this.controls.at(index).get(fieldName) as FormControl;
    //return this.controls.at(index).get(fieldName) as FormControl;
  }
}
