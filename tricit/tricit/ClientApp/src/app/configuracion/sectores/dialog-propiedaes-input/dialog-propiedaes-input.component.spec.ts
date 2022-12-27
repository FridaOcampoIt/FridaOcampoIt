import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogPropiedaesInputComponent } from './dialog-propiedaes-input.component';

describe('DialogPropiedaesInputComponent', () => {
  let component: DialogPropiedaesInputComponent;
  let fixture: ComponentFixture<DialogPropiedaesInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogPropiedaesInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogPropiedaesInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
