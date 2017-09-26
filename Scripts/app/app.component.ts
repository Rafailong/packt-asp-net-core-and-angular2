import { Component } from "@angular/core";

@Component({
  selector: "opengamelist",
  template: `
    <h1>OpenGameList</h1>
    <div class="menu">
      <a class="home" [routerLink]="['']">Home</a> |
      <a class="home" [routerLink]="['about']">About</a> |
      <a class="home" [routerLink]="['login']">Login</a> |
      <a class="home" [routerLink]="['item', 0]">Add New</a>
    </div>
    <router-outlet></router-outlet>
  `
})
export class AppComponent { }