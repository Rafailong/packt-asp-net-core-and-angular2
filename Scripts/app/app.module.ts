import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";

import "rxjs/Rx";

import { AppComponent } from "./app.component";
import { ItemListComponent } from "./items.component";

import { ItemService } from "./item.service";

@NgModule({
  imports: [ BrowserModule, HttpModule ],
  providers: [ ItemService ],
  bootstrap: [ AppComponent ],
  declarations: [ AppComponent, ItemListComponent ],
})
export class AppModule { }