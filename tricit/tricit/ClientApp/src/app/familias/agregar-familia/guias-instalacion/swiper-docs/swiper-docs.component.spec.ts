import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperDocsGIComponent } from './swiper-docs.component';

describe('SwiperDocsComponent', () => {
    let component: SwiperDocsGIComponent;
    let fixture: ComponentFixture<SwiperDocsGIComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [SwiperDocsGIComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwiperDocsGIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
