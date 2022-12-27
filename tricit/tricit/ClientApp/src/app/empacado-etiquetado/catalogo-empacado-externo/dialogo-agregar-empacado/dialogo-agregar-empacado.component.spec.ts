import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarEmpacadoComponent } from './dialogo-agregar-empacado.component';

describe('DialogoAgregarEmpacadoComponent', () => {
  let component: DialogoAgregarEmpacadoComponent;
  let fixture: ComponentFixture<DialogoAgregarEmpacadoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarEmpacadoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarEmpacadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
