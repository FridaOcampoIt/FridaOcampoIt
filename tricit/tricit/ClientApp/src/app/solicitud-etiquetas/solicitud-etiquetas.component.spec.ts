import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolicitudEtiquetasComponent } from './solicitud-etiquetas.component';

describe('SolicitudEtiquetasComponent', () => {
  let component: SolicitudEtiquetasComponent;
  let fixture: ComponentFixture<SolicitudEtiquetasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolicitudEtiquetasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolicitudEtiquetasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
