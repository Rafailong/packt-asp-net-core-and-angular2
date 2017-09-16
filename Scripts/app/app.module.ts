import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule }   from '@angular/forms';
import { HttpModule }    from '@angular/http';

import "rxjs/Rx";

import { AppComponent } from "./app.component";
import { ItemListComponent } from "./items.component";
import { ItemDetailComponent } from "./item-detail.component";

import { ItemService } from "./item.service";

@NgModule({
  imports: [ BrowserModule, HttpModule, FormsModule ],
  providers: [ ItemService ],
  bootstrap: [ AppComponent ],
  declarations: [ AppComponent, ItemListComponent, ItemDetailComponent ],
})
export class AppModule { }