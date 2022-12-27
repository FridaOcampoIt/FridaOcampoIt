import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarConfiguracionReprocesoComponent } from './dialogo-eliminar-reproceso.component';

describe('DialogoEliminarComponent', () => {
  let component: DialogoEliminarConfiguracionReprocesoComponent;
  let fixture: ComponentFixture<DialogoEliminarConfiguracionReprocesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarConfiguracionReprocesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarConfiguracionReprocesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
