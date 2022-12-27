import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteArmadosComponent } from './reporte-armados.component';

describe('ReporteArmadosComponent', () => {
  let component: ReporteArmadosComponent;
  let fixture: ComponentFixture<ReporteArmadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReporteArmadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReporteArmadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
