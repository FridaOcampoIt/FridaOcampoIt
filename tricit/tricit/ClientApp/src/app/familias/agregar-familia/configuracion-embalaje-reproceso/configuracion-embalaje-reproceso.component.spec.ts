import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfiguracionEmbalajeReprocesoComponent } from './configuracion-embalaje-reproceso.component';

describe('ConfiguracionEmbalajeReprocesoComponent', () => {
  let component: ConfiguracionEmbalajeReprocesoComponent;
  let fixture: ComponentFixture<ConfiguracionEmbalajeReprocesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfiguracionEmbalajeReprocesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfiguracionEmbalajeReprocesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
