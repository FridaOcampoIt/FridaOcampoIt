import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoLectorComponent } from './dialogo-lector.component';

describe('DialogoLectorComponent', () => {
  let component: DialogoLectorComponent;
  let fixture: ComponentFixture<DialogoLectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoLectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoLectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
