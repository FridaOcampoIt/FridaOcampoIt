import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoMultiplesComponent } from './dialogo-multiples.component';

describe('DialogoMultiplesComponent', () => {
  let component: DialogoMultiplesComponent;
  let fixture: ComponentFixture<DialogoMultiplesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoMultiplesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoMultiplesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
