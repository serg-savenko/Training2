import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormArray, RequiredValidator, Validators } from '@angular/forms';

@Component({
  selector: 'add-user-with-balance',
  templateUrl: './add-user-with-balance.component.html',
})
export class AddUserWithBalanceComponent implements OnInit {

  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() { }

  baseUrl: string;

  userWithBalance = this.fb.group({
    name: new FormControl('', Validators.required),
    surname: new FormControl('', Validators.required),
    age: new FormControl(0),
    registrationCity: new FormControl(''),
    currency: new FormControl(''),
    balance: this.fb.array([this.fb.group({ date: new FormControl(''), amount: new FormControl(0) })])
  });

  userWithBalanceInitialValues = this.userWithBalance.value;

  balance = this.userWithBalance.get('balance') as FormArray;
  addBalance() {
    this.balance.push(this.fb.group({ date: new FormControl(''), amount: new FormControl(0) }));
  }
  removeBalance(index: number) {
    this.balance.removeAt(index);
  }

  onSubmit() {
    this.http.post<string>(this.baseUrl + 'userwithbalance',
      this.userWithBalance.value).subscribe(
        () => {
          alert("Saved!");
          this.balance.clear();
          this.addBalance();
          this.userWithBalance.reset(this.userWithBalanceInitialValues);
        },
        (e: HttpErrorResponse) => {
          alert(e.message + " : " + JSON.stringify(e.error));
          console.warn(e);
        });
  }

}
