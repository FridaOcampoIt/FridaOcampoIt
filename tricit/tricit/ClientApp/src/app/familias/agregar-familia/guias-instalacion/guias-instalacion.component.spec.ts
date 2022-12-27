import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuiasInstalacionComponent } from './guias-instalacion.component';

describe('GuiasInstalacionComponent', () => {
  let component: GuiasInstalacionComponent;
  let fixture: ComponentFixture<GuiasInstalacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuiasInstalacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuiasInstalacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
