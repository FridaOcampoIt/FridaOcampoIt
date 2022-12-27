import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperVideosGUComponent } from './swiper-videos.component';

describe('SwiperVideosComponent', () => {
  let component: SwiperVideosGUComponent;
  let fixture: ComponentFixture<SwiperVideosGUComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwiperVideosGUComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwiperVideosGUComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
