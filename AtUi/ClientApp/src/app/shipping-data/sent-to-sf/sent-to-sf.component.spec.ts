import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SentToSfComponent } from './sent-to-sf.component';

describe('SentToSfComponent', () => {
  let component: SentToSfComponent;
  let fixture: ComponentFixture<SentToSfComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SentToSfComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SentToSfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
