import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuiasUsoComponent } from './guias-uso.component';

describe('GuiasUsoComponent', () => {
  let component: GuiasUsoComponent;
  let fixture: ComponentFixture<GuiasUsoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuiasUsoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuiasUsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
