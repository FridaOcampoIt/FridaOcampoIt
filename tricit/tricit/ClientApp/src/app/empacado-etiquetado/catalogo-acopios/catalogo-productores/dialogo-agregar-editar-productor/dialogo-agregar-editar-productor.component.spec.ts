import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarEditarProductorComponent } from './dialogo-agregar-editar-productor.component';

describe('DialogoAgregarEditarProductorComponent', () => {
  let component: DialogoAgregarEditarProductorComponent;
  let fixture: ComponentFixture<DialogoAgregarEditarProductorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarEditarProductorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarEditarProductorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
