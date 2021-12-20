import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'edit-user-with-balance',
  templateUrl: './edit-user-with-balance.component.html',
})
export class EditUserWithBalanceComponent implements OnInit {

  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private route: ActivatedRoute, private router: Router) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.http.get(this.baseUrl + 'userwithbalance/' + this.route.snapshot.paramMap.get('id'),
      this.userWithBalance.value).subscribe(
        data => {
          this.userWithBalance.get('id').setValue(+this.route.snapshot.paramMap.get('id'));
          this.userWithBalance.get('name').setValue(data['name']);
          this.userWithBalance.get('surname').setValue(data['surname']);
          this.userWithBalance.get('age').setValue(data['age']);
          this.userWithBalance.get('registrationCity').setValue(data['registrationCity']);
          this.userWithBalance.get('currency').setValue(data['currency']);
          data["balance"].forEach(e => { this.addBalance(e["date"], e["amount"]) });
        },
        error => {
          alert(error.message + " : " + JSON.stringify(error.error));
          console.warn(error);
        });
  }

  baseUrl: string;

  userWithBalance = this.fb.group({
    id: new FormControl(0),
    name: new FormControl('', Validators.required),
    surname: new FormControl('', Validators.required),
    age: new FormControl(0),
    registrationCity: new FormControl(''),
    currency: new FormControl(''),
    balance: this.fb.array([])
  });

  balance = this.userWithBalance.get('balance') as FormArray;
  addBalance(dateValue: string= "", amountValue: number = 0) {
    this.balance.push(this.fb.group({ date: new FormControl(dateValue), amount: new FormControl(amountValue) }));
  }
  removeBalance(index: number) {
    this.balance.removeAt(index);
  }

  onSubmit() {
    this.http.put(this.baseUrl + 'userwithbalance',
      this.userWithBalance.value).subscribe(
        () => {
          alert("Saved!");
        },
        error => {
          alert(error.message + " : " + JSON.stringify(error.error));
          console.warn(error);
        });
  }

}
