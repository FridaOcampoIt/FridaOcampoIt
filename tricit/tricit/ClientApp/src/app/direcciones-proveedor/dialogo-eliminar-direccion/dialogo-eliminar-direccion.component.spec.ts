import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarDireccionComponent } from './dialogo-eliminar-direccion.component';

describe('DialogoEliminarDireccionComponent', () => {
  let component: DialogoEliminarDireccionComponent;
  let fixture: ComponentFixture<DialogoEliminarDireccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarDireccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarDireccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
