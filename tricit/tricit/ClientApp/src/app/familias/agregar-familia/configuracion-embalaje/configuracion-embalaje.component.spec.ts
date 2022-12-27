import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfiguracionEmbalajeComponent } from './configuracion-embalaje.component';

describe('ConfiguracionEmbalajeComponent', () => {
  let component: ConfiguracionEmbalajeComponent;
  let fixture: ComponentFixture<ConfiguracionEmbalajeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfiguracionEmbalajeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfiguracionEmbalajeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
