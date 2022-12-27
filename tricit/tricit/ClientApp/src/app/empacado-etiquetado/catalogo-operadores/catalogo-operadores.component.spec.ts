import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoOperadoresComponent } from './catalogo-operadores.component';

describe('CatalogoOperadoresComponent', () => {
  let component: CatalogoOperadoresComponent;
  let fixture: ComponentFixture<CatalogoOperadoresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoOperadoresComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoOperadoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
