import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AddUserWithBalanceComponent } from './add-user-with-balance/add-user-with-balance.component';
import { EditUserWithBalanceComponent } from './edit-user-with-balance/edit-user-with-balance.component';
import { ListUserWithBalanceComponent } from './list-user-with-balance/list-user-with-balance.component';
import { AddBalanceComponent } from './add-balance/add-balance.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    AddUserWithBalanceComponent,
    EditUserWithBalanceComponent,
    ListUserWithBalanceComponent,
    AddBalanceComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'add-user-with-balance', component: AddUserWithBalanceComponent },
      { path: 'edit-user-with-balance/:id', component: EditUserWithBalanceComponent },
      { path: 'list-user-with-balance', component: ListUserWithBalanceComponent },
      { path: 'add-balance/:id', component: AddBalanceComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
