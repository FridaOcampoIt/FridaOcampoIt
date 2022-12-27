import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoAcopiosComponent } from './catalogo-acopios.component';

describe('CatalogoAcopiosComponent', () => {
  let component: CatalogoAcopiosComponent;
  let fixture: ComponentFixture<CatalogoAcopiosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoAcopiosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoAcopiosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
