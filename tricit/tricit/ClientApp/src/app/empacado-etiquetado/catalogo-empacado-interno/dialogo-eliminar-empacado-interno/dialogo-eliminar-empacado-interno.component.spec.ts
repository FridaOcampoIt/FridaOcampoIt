import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarEmpacadoInternoComponent } from './dialogo-eliminar-empacado-interno.component';

describe('DialogoEliminarEmpacadoInternoComponent', () => {
  let component: DialogoEliminarEmpacadoInternoComponent;
  let fixture: ComponentFixture<DialogoEliminarEmpacadoInternoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarEmpacadoInternoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarEmpacadoInternoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
