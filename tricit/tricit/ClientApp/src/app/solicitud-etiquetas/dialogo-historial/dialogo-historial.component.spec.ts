import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoHistorialComponent } from './dialogo-historial.component';

describe('DialogoHistorialComponent', () => {
  let component: DialogoHistorialComponent;
  let fixture: ComponentFixture<DialogoHistorialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoHistorialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoHistorialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
