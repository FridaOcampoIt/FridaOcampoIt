import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarLineaComponent } from './dialogo-agregar-linea.component';

describe('DialogoAgregarLineaComponent', () => {
  let component: DialogoAgregarLineaComponent;
  let fixture: ComponentFixture<DialogoAgregarLineaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarLineaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarLineaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
