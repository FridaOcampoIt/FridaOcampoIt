import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarConfiguracionReprocesoComponent } from './dialogo-agregar-reproceso.component';

describe('DialogoAgregarComponent', () => {
  let component: DialogoAgregarConfiguracionReprocesoComponent;
  let fixture: ComponentFixture<DialogoAgregarConfiguracionReprocesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarConfiguracionReprocesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarConfiguracionReprocesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
