import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from '@angular/router';

import { Item } from "./item";
import { ItemService } from './item.service';

@Component({
  selector: "item-detail-view",
  template: `
  <div *ngIf="item">
    <h2>
      <a href="#" (click)="onBack()">&laquo; Back to Home</a>
    </h2>
    <div class="item-container">
      <ul class="nav nav-tabs">
        <li role="presentation">
          <a href="#" (click)="onItemDetailEdit(item)">Edit</a>
        </li>
        <li role="presentation" class="active">
          <a href="#">View</a>
        </li>
      </ul>
      <div class="panel panel-default">
        <div class="panel-body">
          <div class="item-image-panel">
            <img src="/img/item-image-sample.png" alt="{{item.Title}}" />
            <div class="caption">Sample image with caption.</div>
          </div>
          <h3>{{item.Title}}</h3>
          <p>{{item.Description}}</p>
          <p>{{item.Text}}</p>
        </div>
      </div>
    </div>
  </div>
  `
})
export class ItemDetailViewComponent implements OnInit {
  item: Item;

  constructor(
    private router: Router,
    private itemService: ItemService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    var id = Number.parseInt(this.activatedRoute.snapshot.params['id']);
    if (id > 0) {
      this.itemService.get(id)
        .then(item => this.item = item);
    } else if (id === 0) {
      console.log("id is 0: switching to edit mode...");
      this.router.navigate(["item/edit", 0]);
    } else {
      this.router.navigate(['']);
    }
  }

  onItemDetailEdit(item: Item) {
    this.router.navigate(['item/edit', item.Id]);
    return false;
  }

  onBack() {
    this.router.navigate(['']);
  }
}