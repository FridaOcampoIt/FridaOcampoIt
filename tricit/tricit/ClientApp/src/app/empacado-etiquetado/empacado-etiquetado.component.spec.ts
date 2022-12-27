import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpacadoEtiquetadoComponent } from './empacado-etiquetado.component';

describe('EmpacadoEtiquetadoComponent', () => {
  let component: EmpacadoEtiquetadoComponent;
  let fixture: ComponentFixture<EmpacadoEtiquetadoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpacadoEtiquetadoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpacadoEtiquetadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
