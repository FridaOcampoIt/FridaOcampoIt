import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarEtiquetaComponent } from './agregar-etiqueta.component';

describe('AgregarEtiquetaComponent', () => {
  let component: AgregarEtiquetaComponent;
  let fixture: ComponentFixture<AgregarEtiquetaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgregarEtiquetaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgregarEtiquetaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
