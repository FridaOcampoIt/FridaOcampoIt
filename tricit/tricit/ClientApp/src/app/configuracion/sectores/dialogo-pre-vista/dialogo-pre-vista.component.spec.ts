import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoPreVistaComponent } from './dialogo-pre-vista.component';

describe('DialogoPreVistaComponent', () => {
  let component: DialogoPreVistaComponent;
  let fixture: ComponentFixture<DialogoPreVistaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoPreVistaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoPreVistaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
