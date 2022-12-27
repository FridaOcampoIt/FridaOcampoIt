import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoEmpacadoExternoComponent } from './catalogo-empacado-externo.component';

describe('CatalogoEmpacadoExternoComponent', () => {
  let component: CatalogoEmpacadoExternoComponent;
  let fixture: ComponentFixture<CatalogoEmpacadoExternoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoEmpacadoExternoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoEmpacadoExternoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
