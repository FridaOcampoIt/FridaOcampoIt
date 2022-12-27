import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperDocsGUComponent } from './swiper-docs.component';

describe('SwiperDocsComponent', () => {
  let component: SwiperDocsGUComponent;
  let fixture: ComponentFixture<SwiperDocsGUComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwiperDocsGUComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwiperDocsGUComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
