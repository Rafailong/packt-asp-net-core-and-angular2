import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";

import "rxjs/Rx";

import { AppComponent } from "./app.component";

@NgModule({
  imports: [ BrowserModule, HttpModule ],
  providers: [],
  bootstrap: [ AppComponent ],
  declarations: [ AppComponent ],
})
export class AppModule { }