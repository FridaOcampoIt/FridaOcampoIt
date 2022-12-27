import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarEtiquetaComponent } from './dialogo-eliminar-etiqueta.component';

describe('DialogoEliminarEtiquetaComponent', () => {
  let component: DialogoEliminarEtiquetaComponent;
  let fixture: ComponentFixture<DialogoEliminarEtiquetaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarEtiquetaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarEtiquetaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
