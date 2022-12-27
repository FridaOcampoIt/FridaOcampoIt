import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoReporteComponent } from './dialogo-reporte.component';

describe('DialogoReporteComponent', () => {
  let component: DialogoReporteComponent;
  let fixture: ComponentFixture<DialogoReporteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoReporteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoReporteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
