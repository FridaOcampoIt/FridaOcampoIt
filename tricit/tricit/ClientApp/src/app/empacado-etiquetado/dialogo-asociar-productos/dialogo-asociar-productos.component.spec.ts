import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAsociarProductosComponent } from './dialogo-asociar-productos.component';

describe('DialogoAsociarProductosComponent', () => {
  let component: DialogoAsociarProductosComponent;
  let fixture: ComponentFixture<DialogoAsociarProductosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAsociarProductosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAsociarProductosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
