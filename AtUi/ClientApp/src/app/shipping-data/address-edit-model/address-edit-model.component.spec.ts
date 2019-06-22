import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressEditModelComponent } from './address-edit-model.component';

describe('AddressEditModelComponent', () => {
  let component: AddressEditModelComponent;
  let fixture: ComponentFixture<AddressEditModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddressEditModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressEditModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
