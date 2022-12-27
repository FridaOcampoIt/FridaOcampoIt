import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoEmpacadoAgroComponent } from './catalogo-empacado-agro.component';

describe('CatalogoEmpacadoExternoComponent', () => {
  let component: CatalogoEmpacadoAgroComponent;
  let fixture: ComponentFixture<CatalogoEmpacadoAgroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoEmpacadoAgroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoEmpacadoAgroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
