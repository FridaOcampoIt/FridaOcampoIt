import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarOperadorComponent } from './dialogo-eliminar-operador.component';

describe('DialogoEliminarOperadorComponent', () => {
  let component: DialogoEliminarOperadorComponent;
  let fixture: ComponentFixture<DialogoEliminarOperadorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarOperadorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarOperadorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
