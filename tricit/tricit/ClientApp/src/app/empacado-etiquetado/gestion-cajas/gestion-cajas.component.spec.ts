import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionCajasComponent } from './gestion-cajas.component';

describe('GestionCajasComponent', () => {
  let component: GestionCajasComponent;
  let fixture: ComponentFixture<GestionCajasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionCajasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionCajasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
