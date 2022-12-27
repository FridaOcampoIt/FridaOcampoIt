import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoProductoresComponent } from './catalogo-productores.component';

describe('CatalogoProductoresComponent', () => {
  let component: CatalogoProductoresComponent;
  let fixture: ComponentFixture<CatalogoProductoresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoProductoresComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoProductoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
