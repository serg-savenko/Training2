import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'list-user-with-balance',
  templateUrl: './list-user-with-balance.component.html'
})
export class ListUserWithBalanceComponent {
  users: UserWithBalance[];

  constructor(private http: HttpClient, private router: Router, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.getUsers();
  }

  baseUrl: string;
  
  getUsers() {
    this.http.get<UserWithBalance[]>(this.baseUrl + 'userwithbalance').subscribe(
      data => {
        this.users = data;
      },
      error => {
        alert(error.message + " : " + JSON.stringify(error.error));
        console.warn(error)
      });

  }

  addBalanceBtnClick(id: number) {
    this.router.navigateByUrl('/add-balance/' + id);
  }


  editBtnClick(id: number) {
    this.router.navigateByUrl('/edit-user-with-balance/' + id);
  }

  deleteBtnClick(id: number) {
    this.http.delete(this.baseUrl + 'userwithbalance/' + id).subscribe(
      () => {
        alert('Done!');
        this.getUsers();
      },
      error => {
        alert(error.message + " : " + JSON.stringify(error.error));
        console.warn(error)
      });

  }
}

interface UserWithBalance {
  id: number;
  name: string;
  surname: string;
  age: number;
  registrationCity: string;
  currency: string;
  balanceNet: number;
  balance: Balance[];
}
interface Balance {
  date: string;
  amount: number;
}
