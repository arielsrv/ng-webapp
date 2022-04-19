import {async, ComponentFixture, TestBed, waitForAsync} from '@angular/core/testing';

import {CounterComponent} from './counter.component';

describe('CounterComponent', () => {
  let fixture: ComponentFixture<CounterComponent>;

  function setUp() {
    TestBed.configureTestingModule({
      declarations: [CounterComponent]
    })
      .compileComponents();
    fixture = TestBed.createComponent(CounterComponent);
    fixture.detectChanges();
  }

  beforeEach(() => {
    setUp();
  });

  it('should display a title', (() => {
    const titleText = fixture.nativeElement.querySelector('h1').textContent;
    expect(titleText).toEqual('Counter');
  }));

  it('should start with count 0, then increments by 1 when clicked', (() => {
    const countElement = fixture.nativeElement.querySelector('strong');
    expect(countElement.textContent).toEqual('0');

    const incrementButton = fixture.nativeElement.querySelector('button');
    incrementButton.click();
    fixture.detectChanges();
    expect(countElement.textContent).toEqual('1');
  }));
});
