import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoSeguimientoComponent } from './dialogo-seguimiento.component';

describe('DialogoSeguimientoComponent', () => {
  let component: DialogoSeguimientoComponent;
  let fixture: ComponentFixture<DialogoSeguimientoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoSeguimientoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoSeguimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
