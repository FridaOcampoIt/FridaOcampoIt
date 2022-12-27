import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperVideosGIComponent } from './swiper-videos.component';

describe('SwiperVideosComponent', () => {
  let component: SwiperVideosGIComponent;
    let fixture: ComponentFixture<SwiperVideosGIComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [SwiperVideosGIComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(SwiperVideosGIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
