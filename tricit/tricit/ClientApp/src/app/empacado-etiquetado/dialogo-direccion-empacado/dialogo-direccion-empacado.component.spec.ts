import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoDireccionEmpacadoComponent } from './dialogo-direccion-empacado.component';

describe('DialogoDireccionEmpacadoComponent', () => {
  let component: DialogoDireccionEmpacadoComponent;
  let fixture: ComponentFixture<DialogoDireccionEmpacadoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoDireccionEmpacadoComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoDireccionEmpacadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
