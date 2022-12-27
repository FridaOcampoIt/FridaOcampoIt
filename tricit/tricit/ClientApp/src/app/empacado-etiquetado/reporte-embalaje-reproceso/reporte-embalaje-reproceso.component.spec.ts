import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteEmbalajeReprocesoComponent } from './reporte-embalaje-reproceso.component';

describe('ReporteEmbalajeReprocesoComponent', () => {
  let component: ReporteEmbalajeReprocesoComponent;
  let fixture: ComponentFixture<ReporteEmbalajeReprocesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReporteEmbalajeReprocesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReporteEmbalajeReprocesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
