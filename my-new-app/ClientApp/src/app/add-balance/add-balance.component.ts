import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'add-balance',
  templateUrl: './add-balance.component.html',
})
export class AddBalanceComponent implements OnInit {

  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private route: ActivatedRoute, private router: Router) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.http.get(this.baseUrl + 'userwithbalance/' + this.route.snapshot.paramMap.get('id')).subscribe(
        data => {
          this.user = data;
        },
        error => {
          alert(error.message + " : " + JSON.stringify(error.error));
          console.warn(error);
        });
  }

  baseUrl: string;
  user: any;

  formBalance = this.fb.group({
    date: new FormControl('', Validators.required),
    amount: new FormControl(0, Validators.required),
  });

  onSubmit() {
    this.http.post(this.baseUrl + 'userwithbalance/' + this.route.snapshot.paramMap.get('id') + '/balance',
      this.formBalance.value).subscribe(
        data => {
          alert("Added!");
          this.user = data;
          this.formBalance.reset();
        },
        error => {
          alert(error.message + " : " + JSON.stringify(error.error));
          console.warn(error);
        });
  }

}
