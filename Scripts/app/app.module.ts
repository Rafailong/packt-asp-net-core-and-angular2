import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { HttpModule }    from '@angular/http';
import { RouterModule } from "@angular/router";

import "rxjs/Rx";

import { BsRootModule } from 'ngx-bootstrap';

import { AppRouting } from './app.routing';
import { AppComponent } from "./app.component";
import { HomeComponent } from './home.component';
import { AboutComponent } from './about.component';
import { LoginComponent } from './login.component';
import { PageNotFoundComponent } from './page-not-found.component';
import { ItemListComponent } from "./items.component";
import { ItemDetailEditComponent } from "./item-detail-edit.component";
import { ItemDetailViewComponent } from './item-detail-view.component';

import { ItemService } from "./item.service";

@NgModule({
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    AppRouting,
    BsRootModule
  ],
  providers: [ ItemService ],
  bootstrap: [ AppComponent ],
  declarations: [ 
    AppComponent,
    HomeComponent,
    AboutComponent,
    LoginComponent,
    PageNotFoundComponent,
    ItemListComponent,
    ItemDetailViewComponent,
    ItemDetailEditComponent
  ],
})
export class AppModule { }