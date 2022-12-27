import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DireccionesProveedorComponent } from './direcciones-proveedor.component';

describe('DireccionesProveedorComponent', () => {
  let component: DireccionesProveedorComponent;
  let fixture: ComponentFixture<DireccionesProveedorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DireccionesProveedorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DireccionesProveedorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
