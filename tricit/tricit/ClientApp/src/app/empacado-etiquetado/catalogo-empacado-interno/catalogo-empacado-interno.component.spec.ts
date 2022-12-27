import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoEmpacadoInternoComponent } from './catalogo-empacado-interno.component';

describe('CatalogoEmpacadoInternoComponent', () => {
  let component: CatalogoEmpacadoInternoComponent;
  let fixture: ComponentFixture<CatalogoEmpacadoInternoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoEmpacadoInternoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoEmpacadoInternoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
