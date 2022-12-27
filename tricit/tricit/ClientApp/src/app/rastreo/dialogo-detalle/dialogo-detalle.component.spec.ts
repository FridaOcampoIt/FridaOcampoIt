import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoDetalleComponent } from './dialogo-detalle.component';

describe('DialogoDetalleComponent', () => {
  let component: DialogoDetalleComponent;
  let fixture: ComponentFixture<DialogoDetalleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoDetalleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoDetalleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
