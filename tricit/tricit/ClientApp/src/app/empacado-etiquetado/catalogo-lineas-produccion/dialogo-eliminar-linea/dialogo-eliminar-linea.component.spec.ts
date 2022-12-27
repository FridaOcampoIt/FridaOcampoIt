import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarLineaComponent } from './dialogo-eliminar-linea.component';

describe('DialogoEliminarLineaComponent', () => {
  let component: DialogoEliminarLineaComponent;
  let fixture: ComponentFixture<DialogoEliminarLineaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarLineaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarLineaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
