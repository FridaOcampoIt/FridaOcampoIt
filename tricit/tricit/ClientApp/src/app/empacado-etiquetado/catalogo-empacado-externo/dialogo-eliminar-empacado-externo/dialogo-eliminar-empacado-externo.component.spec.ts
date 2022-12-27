import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarEmpacadoExternoComponent } from './dialogo-eliminar-empacado-externo.component';

describe('DialogoEliminarEmpacadoExternoComponent', () => {
  let component: DialogoEliminarEmpacadoExternoComponent;
  let fixture: ComponentFixture<DialogoEliminarEmpacadoExternoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarEmpacadoExternoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarEmpacadoExternoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
