import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditUserWithBalanceComponent } from './edit-user-with-balance.component';

describe('EditUserWithBalanceComponent', () => {
  let component: EditUserWithBalanceComponent;
  let fixture: ComponentFixture<EditUserWithBalanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditUserWithBalanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditUserWithBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
