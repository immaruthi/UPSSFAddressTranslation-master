import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressBookEditModelComponent } from './address-book-edit-model.component';

describe('AddressBookEditModelComponent', () => {
  let component: AddressBookEditModelComponent;
  let fixture: ComponentFixture<AddressBookEditModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddressBookEditModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressBookEditModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
