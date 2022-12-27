import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogAsignarFormularioComponent } from './dialog-asignar-formulario.component';

describe('DialogAsignarFormularioComponent', () => {
  let component: DialogAsignarFormularioComponent;
  let fixture: ComponentFixture<DialogAsignarFormularioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogAsignarFormularioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogAsignarFormularioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
