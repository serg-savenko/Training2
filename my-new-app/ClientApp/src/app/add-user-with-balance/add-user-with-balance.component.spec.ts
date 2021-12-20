import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserWithBalanceComponent } from './add-user-with-balance.component';

describe('UserWithBalanceComponent', () => {
  let component: AddUserWithBalanceComponent;
  let fixture: ComponentFixture<AddUserWithBalanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUserWithBalanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserWithBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
