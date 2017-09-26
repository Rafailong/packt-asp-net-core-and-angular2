import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from '@angular/router';

import { Item } from "./item";
import { ItemService } from './item.service';

@Component({
  selector: "item-detail-view",
  template: `
    <div *ngIf="item" class="item-details">
      <h2>{{item.Title}}</h2>
      <p>{{item.Description}}</p>
    </div>
  `,
  styles: [`
    .item-details {
      margin: 5px;
      padding: 5px 10px;
      border: 1px solid black;
      background-color: #dddddd;
      width: 300px;
    }
    .item-details * {
      vertical-align: middle;
    }
    .item-details ui li {
      padding: 5px 0;
    }
  `]
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
}