import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GarantiaServicioComponent } from './garantia-servicio.component';

describe('GarantiaServicioComponent', () => {
  let component: GarantiaServicioComponent;
  let fixture: ComponentFixture<GarantiaServicioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GarantiaServicioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GarantiaServicioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
