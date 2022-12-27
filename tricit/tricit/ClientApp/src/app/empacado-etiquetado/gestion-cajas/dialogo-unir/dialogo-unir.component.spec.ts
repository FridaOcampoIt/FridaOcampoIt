import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoUnirComponent } from './dialogo-unir.component';

describe('DialogoUnirComponent', () => {
  let component: DialogoUnirComponent;
  let fixture: ComponentFixture<DialogoUnirComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoUnirComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoUnirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
