import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarProveedorComponent } from './dialogo-agregar-proveedor.component';

describe('DialogoAgregarProveedorComponent', () => {
  let component: DialogoAgregarProveedorComponent;
  let fixture: ComponentFixture<DialogoAgregarProveedorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarProveedorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarProveedorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
