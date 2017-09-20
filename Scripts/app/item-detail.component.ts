import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from '@angular/router';

import { Item } from "./item";
import { ItemService } from './item.service';

@Component({
  selector: "item-detail",
  template: `
    <div *ngIf="item" class="item-details">
      <h2>{{item.Title}} - Detail View</h2>
      <ul>
        <li>
          <label>Title:</label>
          <input [(ngModel)]="item.Title" placeholder="Insert the title..." />
        </li>
        <li>
          <textarea [(ngModel)]="item.Description" placeholder="Insert a suitable description">
          </textarea>
        </li>
      </ul>
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
export class ItemDetailComponent implements OnInit {
  item: Item;

  constructor(
    private router: Router,
    private itemService: ItemService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    var id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.itemService.get(id)
        .then(item => this.item = item);
    } else {
      this.router.navigate(['']);
    }
  }
}