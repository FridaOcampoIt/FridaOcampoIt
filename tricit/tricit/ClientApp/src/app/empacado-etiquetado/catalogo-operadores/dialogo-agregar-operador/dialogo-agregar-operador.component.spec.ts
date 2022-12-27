import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarOperadorComponent } from './dialogo-agregar-operador.component';

describe('DialogoAgregarOperadorComponent', () => {
  let component: DialogoAgregarOperadorComponent;
  let fixture: ComponentFixture<DialogoAgregarOperadorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarOperadorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarOperadorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
