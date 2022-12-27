import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogoDistribuidoresComponent } from './catalogo-distribuidores.component';

describe('CatalogoDistribuidoresComponent', () => {
  let component: CatalogoDistribuidoresComponent;
  let fixture: ComponentFixture<CatalogoDistribuidoresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CatalogoDistribuidoresComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogoDistribuidoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
