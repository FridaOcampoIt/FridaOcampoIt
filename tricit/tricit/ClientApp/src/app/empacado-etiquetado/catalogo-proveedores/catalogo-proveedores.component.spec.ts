import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoProveedoresComponent } from './catalogo-proveedores.component';

describe('CatalogoProveedoresComponent', () => {
  let component: CatalogoProveedoresComponent;
  let fixture: ComponentFixture<CatalogoProveedoresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoProveedoresComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoProveedoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
