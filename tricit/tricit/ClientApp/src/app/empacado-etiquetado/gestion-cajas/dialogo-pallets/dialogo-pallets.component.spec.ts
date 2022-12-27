import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoPalletsComponent } from './dialogo-pallets.component';

describe('DialogoPalletsComponent', () => {
  let component: DialogoPalletsComponent;
  let fixture: ComponentFixture<DialogoPalletsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoPalletsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoPalletsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
