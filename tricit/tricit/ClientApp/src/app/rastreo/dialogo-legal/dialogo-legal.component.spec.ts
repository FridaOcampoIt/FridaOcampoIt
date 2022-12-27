import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoLegalComponent } from './dialogo-legal.component';

describe('DialogoLegalComponent', () => {
  let component: DialogoLegalComponent;
  let fixture: ComponentFixture<DialogoLegalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoLegalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoLegalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
