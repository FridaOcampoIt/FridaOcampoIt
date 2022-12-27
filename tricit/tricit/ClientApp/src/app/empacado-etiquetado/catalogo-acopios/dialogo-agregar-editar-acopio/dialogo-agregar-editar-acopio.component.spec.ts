import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarEditarAcopioComponent } from './dialogo-agregar-editar-acopio.component';

describe('DialogoAgregarEditarAcopioComponent', () => {
  let component: DialogoAgregarEditarAcopioComponent;
  let fixture: ComponentFixture<DialogoAgregarEditarAcopioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarEditarAcopioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarEditarAcopioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
