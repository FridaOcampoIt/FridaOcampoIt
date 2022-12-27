import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarDireccionComponent } from './dialogo-agregar-direccion.component';

describe('DialogoAgregarDireccionComponent', () => {
  let component: DialogoAgregarDireccionComponent;
  let fixture: ComponentFixture<DialogoAgregarDireccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarDireccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarDireccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
