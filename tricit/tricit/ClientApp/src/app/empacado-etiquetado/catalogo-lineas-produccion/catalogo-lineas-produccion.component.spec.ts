import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoLineasProduccionComponent } from './catalogo-lineas-produccion.component';

describe('CatalogoLineasProduccionComponent', () => {
  let component: CatalogoLineasProduccionComponent;
  let fixture: ComponentFixture<CatalogoLineasProduccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoLineasProduccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoLineasProduccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
